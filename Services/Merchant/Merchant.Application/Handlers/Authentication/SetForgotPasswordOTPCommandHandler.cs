using Merchants.Application.Commands;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
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
using static EmailManager.MailService.Mail;

namespace Merchants.Application.Handlers.Authentication
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
        private readonly INotificationRepo _notification;

        public SetForgotPasswordOTPCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<SetForgotPasswordOTPCommandHandler> logger,IRedisCacheService redisCacheService, IConfiguration configuration,Mail mail,INotificationRepo notification)
        {
            _mapper = mapper;
            _logger = logger;
            _redisCacheService = redisCacheService;
            //_redisCacheService = redisCacheService;
            _configuration = configuration;
            _mail = mail;
            _notification = notification;
            _userRepository = userRepository;
        }
        public async Task<Response> Handle(SetForgotPasswordOTPCommand request, CancellationToken cancellationToken)
        {
            await _notification.NotificationToManagement("0", "Test");

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

            //await _mail.PublishEmailToQueueAsync(request.email, "OTP For Change Password", $"Dear {user.FirstName},\n Your OTP For Change Password is : {OTP.Number}");
            Email mailComplainer = new Email();
            mailComplainer.to = request.email;
            mailComplainer.subject = $"BeEnergy | One-Time Password";

            mailComplainer.body = $"Dear User, Your OTP for Change Password is {OTP.Number}.";
            await _mail.PublishEmailToQueueAsync(mailComplainer);
            _logger.LogInformation($" Email Sent To Queue");

            await _redisCacheService.SetCacheValueAsync($"OTP:{OTP.Number}", OTP, TimeSpan.FromMinutes(10));
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
