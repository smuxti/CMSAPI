using Merchants.Application.Commands;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Authentication
{
    public class GetForgotPasswordOTPQueryHandler : IRequestHandler<GetForgotPasswordOTPQuery, Response>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IUserRepository _userRepository;
        //private readonly IRedisCacheService _redisCacheService;
        private readonly IConfiguration _configuration;
        public GetForgotPasswordOTPQueryHandler(IUserRepository userRepository, IMapper mapper, ILogger<GetForgotPasswordOTPQueryHandler> logger, IRedisCacheService redisCacheService, IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _redisCacheService = redisCacheService;
            //_redisCacheService = redisCacheService;
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task<Response> Handle(GetForgotPasswordOTPQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            _logger.LogInformation($"GET OTP by Email request {request.email}");

            var otp = await _redisCacheService.GetCacheValueAsynca($"OTP:{request.OTP}");
            if (!string.IsNullOrEmpty(otp))
            {
                var otpresp = JsonSerializer.Deserialize<OtpResponse>(otp);
                if (otpresp == null)
                {
                    response.isSuccess = false;
                    response.ResponseDescription = "Invalid OTP";
                    response.ResponseCode = 0;
                    response.Data = null;
                    return response;
                }
                if (otpresp.Email != request.email)
                {
                    response.isSuccess = false;
                    response.ResponseDescription = "Invalid OTP";
                    response.ResponseCode = 0;
                    response.Data = null;
                    return response;
                }

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "OTP Found Successfully.";
                response.Data = otpresp;
                return response;
            }
            response.isSuccess = false;
            response.ResponseDescription = "OTP Not Found..";
            response.ResponseCode = 0;
            response.Data = null;
            return response;


        }
    }
}
