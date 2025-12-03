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
    public class AddMerchantLocationHandler : IRequestHandler<AddMerchantLocationCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IZones _zoneRepository;

        public AddMerchantLocationHandler(IZones zoneRepository, IMapper mapper,
            ILogger<AddMerchantLocationHandler> logger, IHttpContextAccessor httpContextAccessor)
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
        public async Task<Response> Handle(AddMerchantLocationCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var merchantLocation = _mapper.Map<Merchants.Core.Entities.ManagementHierarchy>(request);

                if (merchantLocation.ParentID > 0)
                {
                    var resP = await _zoneRepository.GetZoneByID(merchantLocation.ParentID.Value);
                    if (resP == null)
                    {
                        response.isSuccess = false;
                        response.ResponseDescription = "Invalid zone.";
                        response.ResponseCode = 0;
                        response.Data = null;
                        return response;

                    }
                }
                //var merc = await _zoneRepository.GetZoneByName(merchantLocation.Name);
                //if (merc == null)
                //{
                //    response.isSuccess = false;
                //    response.ResponseDescription = "Location already exists.";
                //    response.ResponseCode = 0;
                //    response.Data = null;
                //    return response;
                //}
                //var type = merchantLocation.ParentID != null ? 2 : 3;

                merchantLocation.ManagementType = request.ManagementType;
                merchantLocation.Status = "Active";
                merchantLocation.isDeleted = false;

                var resp = await _zoneRepository.AddZoneAsync(merchantLocation);

                response.isSuccess = true;
                response.ResponseDescription = "Merchant Location created successfully.";
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
