using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Application.Commands.Merchant;
using Merchants.Application.Commands.MerchantLocation;
using Merchants.Application.Exceptions;
using Merchants.Application.Handlers.Merchant;
using Merchants.Application.Queries;
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
    public class GetZonesHanlder : IRequestHandler<GetZonesCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IZones _zoneRepository;

        public GetZonesHanlder(IZones zoneRepository, IMapper mapper,
            ILogger<GetZonesHanlder> logger, IHttpContextAccessor httpContextAccessor)
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
        public async Task<Response> Handle(GetZonesCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();

            try
            {
                var zones = await _zoneRepository.GetZoneAsyn();
                //MerchantToBeDeleted.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "zones fetch Successfully.";
                response.Data = zones;

                _logger.LogInformation($"Type {zones} deleted successfully.");


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Zones selection failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
