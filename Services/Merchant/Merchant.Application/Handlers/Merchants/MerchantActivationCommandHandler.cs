using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Merchants;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Merchants
{
    public class MerchantActivationCommandHandler : IRequestHandler<MerchantActivationCommand, Response>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IMerchantRepository _merchantRepository;
        //private readonly IRedisCacheService _redisCacheService;
        private readonly IConfiguration _configuration;
        public MerchantActivationCommandHandler(IMerchantRepository merchantRepository, IMapper mapper, 
            ILogger<MerchantActivationCommandHandler> logger, IRedisCacheService redisCacheService, IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _redisCacheService = redisCacheService;
            //_redisCacheService = redisCacheService;
            _configuration = configuration;
            _merchantRepository = merchantRepository;
        }
        public async Task<Response> Handle(MerchantActivationCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            _logger.LogInformation($"Merchant Activation Handler for: {request.Email}");

            var account = await _redisCacheService.GetCacheValueAsynca($"Id:{request.Id}");
            if (!string.IsNullOrEmpty(account))
            {
                var accountResponse = JsonSerializer.Deserialize<accountResponse>(account);
                if (accountResponse == null)
                {
                    response.isSuccess = false;
                    response.ResponseDescription = "Invalid Id";
                    response.ResponseCode = 0;
                    response.Data = null;
                    return response;
                }
                if (accountResponse.Email != request.Email)
                {
                    response.isSuccess = false;
                    response.ResponseDescription = "Invalid Id";
                    response.ResponseCode = 0;
                    response.Data = null;
                    return response;
                }

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Id Found Successfully.";
                response.Data = accountResponse;
                return response;
            }
            response.isSuccess = false;
            response.ResponseDescription = "Id Not Found..";
            response.ResponseCode = 0;
            response.Data = null;
            return response;


        }
    }
}
