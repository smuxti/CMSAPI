using AutoMapper;
using EmailManager.MailService;
using MediatR;
using Merchants.Application.Commands.Merchants;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Core.OneLink;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Merchants.Application.Handlers.Merchants
{
    public class SendActivationEmailCommandHandler : IRequestHandler<SendActivationEmailCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMerchantRepository _merchantRepository;
        private readonly Mail _mail;
        private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        private readonly IRedisCacheService _redisCacheService;


        public SendActivationEmailCommandHandler(IMerchantRepository merchantRepository, IMapper mapper,
            ILogger<SendActivationEmailCommandHandler> logger, IHttpContextAccessor httpContextAccessor,
            Mail mail, IConfiguration configuration, IRedisCacheService redisCacheService)
        {
            _merchantRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mail = mail;
            _configuration = configuration;
            //_baseUrl = _configuration["Urls:ActivationUrl"];
            _redisCacheService = redisCacheService;
        }
        public async Task<Response> Handle(SendActivationEmailCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                _logger.LogInformation($"Base url: {request.BaseUrl}");
                var merchant = await _merchantRepository.GetMerchantByEmail(request.Email);

                if (merchant is not null)
                {
                    //var checkAccount = await _redisCacheService?.GetCacheValueAsynca($"Id:{merchant.Id}");

                    //if (checkAccount is not null)
                    //{
                    //    _logger.LogError($"Merchant already awaiting activation.");

                    //    response.isSuccess = false;
                    //    response.ResponseCode = 0;
                    //    response.ResponseDescription = "Merchant already awaiting activation.";
                    //    response.Data = null;
                    //    return response;
                    //}

                    if (merchant.Status != "Active")
                    {
                        string base64String = Convert.ToBase64String(merchant.Id.ToByteArray());

                        // Apply URL-safe Base64 transformations
                        base64String = base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');

                        _logger.LogInformation($"Merchant {merchant.Id} => {base64String} converted to base64string.");

                        var checkAccount = await _redisCacheService?.GetCacheValueAsynca($"Id:{merchant.Id}");
                        _logger.LogInformation("Checked Redis Cache");
                        if (checkAccount != null)
                        {
                            _logger.LogInformation("Redis Cache not empty, Deserializing");
                            var accountResponse = JsonSerializer.Deserialize<accountResponse>(checkAccount);

                            if (accountResponse is not null &&
                               accountResponse.Timestamp is not null &&
                               accountResponse.Timestamp > DateTime.Now)
                            {
                                _logger.LogInformation("Already in cache!");

                                TimeSpan difference = (TimeSpan)(accountResponse.Timestamp - DateTime.Now);
                                _logger.LogError($"Please wait {difference.Minutes} minutes before sending another email to this merchant.");

                                response.isSuccess = false;
                                response.ResponseCode = 0;
                                response.ResponseDescription = $"Please wait {difference.Minutes} minutes before sending another email.";
                                response.Data = null;
                                return response;
                            }
                            
                            var deleteKey = await _redisCacheService.DeleteCacheValueAsync($"Id:{merchant.Id}");
                        }

                        dynamic account = new System.Dynamic.ExpandoObject();
                        account.Timestamp = DateTime.Now.AddHours(1);
                        account.Email = merchant.Email;
                        account.Id = merchant.Id;

                        await _redisCacheService.SetCacheValueAsync($"Id:{merchant.Id}", account, TimeSpan.FromDays(3));
                        var obj = await _redisCacheService.GetCacheValueAsynca($"Id:{merchant.Id}");
                        var otpresp = JsonSerializer.Deserialize<accountResponse>(obj);

                        _logger.LogInformation($"Merchant Id sent to cache!");
                        _logger.LogInformation($"Generating email...");

                        await _mail.SendEmailAsync(
                            merchant.Email,
                            $"Welcome {merchant.Name} to Raast Web Portal!",
                            $"<p>Dear {merchant.Name},</p>" +
                            "<p>Your Merchant Account is awaiting verification, click the link below to activate your account:</p>" +
                            $"<ul><li><strong>Activate:</strong> <href>{request.BaseUrl}?encodedId={base64String}</href></li></ul>" +
                            "<p>Kindly activate your account within 3 days..</p>" +
                            "<p> </p>" +
                            "<p>Best Regards,</p>" +
                            "<p>Mak Global Payment Solutions.</p>"
                        );

                        _logger.LogInformation($"Email sent! Recipent: {merchant.Email}");
                        _logger.LogInformation($"Activation Link: {request.BaseUrl}?encodedId={base64String}");

                        response.isSuccess = true;
                        response.ResponseCode = 1;
                        response.ResponseDescription = "Activation email sent.";
                        response.Data = merchant;

                        return response;

                    }
                    else
                    {
                        _logger.LogError($"Merchant already active");

                        response.isSuccess = false;
                        response.ResponseCode = 0;
                        response.ResponseDescription = "Merchant already active.";
                        response.Data = null;
                        return response;
                    }
                }
                else
                {
                    _logger.LogError($"Merchant email not found.");

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = "Merchant email not found.";
                    response.Data = null;
                    return response;
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unhandled Exception Occured: {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
