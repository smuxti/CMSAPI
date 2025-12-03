using Authentication.Application.Commands;
using Authentication.Application.Responses;
using Authentication.Core.Interfaces;
using AutoMapper;
using EmailManager.MailService;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Authentication.Application.Handlers
{
    public class SetForgotPasswordOTPCommandHandler : IRequestHandler<SetForgotPasswordOTPCommand, Response>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IUserRepository _userRepository;
        //private readonly IRedisCacheService _redisCacheService;
        private readonly IConfiguration _configuration;
        private readonly Mail _mail;

        public SetForgotPasswordOTPCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<SetForgotPasswordOTPCommandHandler> logger,IRedisCacheService redisCacheService, IConfiguration configuration,Mail mail)
        {
            _mapper = mapper;
            _logger = logger;
            _redisCacheService = redisCacheService;
            //_redisCacheService = redisCacheService;
            _configuration = configuration;
            _mail = mail;
            _userRepository = userRepository;
        }
        public async Task<Response> Handle(SetForgotPasswordOTPCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            _logger.LogInformation($"SET OTP by Email request {request.email}");
            var user = await _userRepository.GetUserByEmail(request.email);

            if (user == null)
            {
                response.isSuccess = false;
                response.ResponseDescription = "Invalid Email";
                response.ResponseCode = 0;
                response.Data = null;
                return response;
            } 

            dynamic OTP = new System.Dynamic.ExpandoObject();
            Random generator = new Random();

            OTP.Number = generator.Next(0, 1000000).ToString("D6");
            OTP.Email = request.email;

            await _mail.SendEmailAsync(request.email, "OTP For Change Password", $"Dear {user.FirstName},\n Your OTP For Change Password is : {OTP.Number}");

            await _redisCacheService.SetCacheValueAsync($"OTP:{OTP.Number}",OTP, TimeSpan.FromMinutes(10));
            var obj = await _redisCacheService.GetCacheValueAsynca($"OTP:{OTP.Number}");
            var otpresp = JsonSerializer.Deserialize<OtpResponse>(obj);

            response.isSuccess = true;
            response.ResponseCode = 1;
            response.ResponseDescription = "OTP Generated";
            response.Data = otpresp;
            return response;

        }
    }
}
