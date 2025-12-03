using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Application.Commands.Merchant;
using Merchants.Application.Commands.MerchantLocation;
using Merchants.Application.Handlers.Merchant;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.MerchantLocation
{
    public class UpdateMerchantLocationHandler : IRequestHandler<UpdateMerchantLocationCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IZones _zoneRepository;

        public UpdateMerchantLocationHandler(IZones zoneRepository, IMapper mapper,
            ILogger<UpdateMerchantLocationHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _zoneRepository = zoneRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            //_mail = mail;
            //_configuration = configuration;
            ////_baseUrl = _configuration["Urls:ActivationUrl"];
            //_redisCacheService = redisCacheService;
        }
        public async Task<Response> Handle(UpdateMerchantLocationCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var merchantLocation = _mapper.Map<Merchants.Core.Entities.ManagementHierarchy>(request);

                var res = _zoneRepository.GetById(request.ID);
                if(res == null)
                {
                    response.isSuccess = false;
                    response.ResponseDescription = "Invalid location.";
                    response.ResponseCode = 0;
                    response.Data = null;
                    return response;
                }

                if (merchantLocation.ParentID > 0)
                {
                    var resP = _zoneRepository.GetZoneByID(merchantLocation.ParentID.Value);
                    if (resP == null)
                    {
                        response.isSuccess = false;
                        response.ResponseDescription = "Invalid zone.";
                        response.ResponseCode = 0;
                        response.Data = null;
                        return response;

                    }
                }
                var type = merchantLocation.ParentID != null ? 2 : 3;

                merchantLocation.ManagementType = type;


                merchantLocation.isDeleted = request.IsDeleted;
                if(merchantLocation.isDeleted == true)
                    merchantLocation.Status = "Active";

                var resp = await _zoneRepository.AddZoneAsync(merchantLocation);

                response.isSuccess = true;
                response.ResponseDescription = "Merchant Location updated successfully.";
                response.ResponseCode = 1;
                response.Data = resp;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Merchant   addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }

    }
}
