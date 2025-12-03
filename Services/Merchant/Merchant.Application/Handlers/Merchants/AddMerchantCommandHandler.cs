using AutoMapper;
using EmailManager.MailService;
using MediatR;
using Merchants.Application.Commands.Merchants;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Merchants.Application.Handlers.Merchants
{
    internal class AddMerchantCommandHandler : IRequestHandler<AddMerchantCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMerchantRepository _merchantRepository;
        private readonly Mail _mail;
        private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        private readonly IRedisCacheService _redisCacheService;


        public AddMerchantCommandHandler(IMerchantRepository merchantRepository, IMapper mapper, 
            ILogger<AddMerchantCommandHandler> logger, IHttpContextAccessor httpContextAccessor, 
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
        public async Task<Response> Handle(AddMerchantCommand request, CancellationToken cancellationToken)
        {
            Response response= new Response();
            try
            {
                var checkmerchant = await _merchantRepository.GetByMerchantCode(request.MerchantCode);
                if (checkmerchant != null) {
                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = "Merchant Code Already Exists.";
                    response.Data = null;
                    return response;
                }
                var merchantEntity = _mapper.Map<Merchant>(request);
                merchantEntity.Posted = 0;
                //merchantEntity.ReasonCode = "Created";
                merchantEntity.CreatedBy= Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                //merchantEntity.CreatedBy = new Guid();
                var generatedMerchant = await _merchantRepository.AddAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Merchant Created Successfully.";
                response.Data = generatedMerchant;

                _logger.LogInformation($"Merchant {merchantEntity} added successfully.");

                if (generatedMerchant != null)
                {
                    //await _mail.SendEmailAsync(request.Email, $"Welcome {request.Name} to Raast Web Portal!", 
                    //    $"Dear {request.Name}," 
                    //    + $"\n\nYour Merchant Account has been activated.\nYour account details are as follows:\n\nUsername: {request.ShortName}\nPassword: Default@1234"
                    //    + "\n\nPlease change your password at the earliest to avoid any inconvieniences."
                    //    + "\n\nMak Global Payment Solutions.");
                    //var encodedId = Convert.ToBase64String(generatedMerchant.Id.ToByteArray())
                    //    .Replace("+", "-") 
                    //    .Replace("/", "_")
                    //    .TrimEnd('=');

                    string base64String = Convert.ToBase64String(generatedMerchant.Id.ToByteArray());

                    // Apply URL-safe Base64 transformations
                    base64String = base64String.Replace('+', '-').Replace('/', '_').TrimEnd('=');

                    _logger.LogInformation($"Merchant {generatedMerchant.Id} => {base64String} converted to base64string.");

                    dynamic account = new System.Dynamic.ExpandoObject();
                    account.Timestamp = DateTime.Now;
                    account.Email = generatedMerchant.Email;
                    account.Id = generatedMerchant.Id;

                    await _redisCacheService.SetCacheValueAsync($"Id:{generatedMerchant.Id}", account, TimeSpan.FromDays(3));
                    var obj = await _redisCacheService.GetCacheValueAsynca($"Id:{generatedMerchant.Id}");
                    var otpresp = JsonSerializer.Deserialize<accountResponse>(obj);

                    _logger.LogInformation($"Merchant Id sent to cache!");
                    _logger.LogInformation($"Generating email...");

                    //await _mail.SendEmailAsync(
                    //    generatedMerchant.Email,
                    //    $"Welcome {generatedMerchant.Name} to Raast Web Portal!",
                    //    $"<p>Dear {generatedMerchant.Name},</p>" +
                    //    "<p>Your Merchant Account is awaiting verification, click the link below to activate your account:</p>" +
                    //    $"<ul><li><strong>Activate:</strong> <href>{request.BaseUrl}?encodedId={base64String}</href></li></ul>" +
                    //    "<p>Kindly activate your account within 3 days..</p>" +
                    //    "<p> </p>" +
                    //    "<p>Best Regards,</p>" +
                    //    "<p>Mak Global Payment Solutions.</p>"
                    //);

                    Mail.Email test = new Mail.Email();
                    test.subject = $"Welcome {generatedMerchant.Name} to Raast Web Portal!";
                    test.cc = "";
                    test.body = $"<p>Dear {generatedMerchant.Name},</p>" +
                        "<p>Your Merchant Account is awaiting verification, click the link below to activate your account:</p>" +
                        $"<ul><li><strong>Activate:</strong> <href>{request.BaseUrl}?encodedId={base64String}</href></li></ul>" +
                        "<p>Kindly activate your account within 3 days..</p>" +
                        "<p> </p>" +
                        "<p>Best Regards,</p>" +
                        "<p>Mak Global Payment Solutions.</p>";
                    test.to = generatedMerchant.Email;
                    test.isHtml = true;
                    await _mail.PublishEmailToQueueAsync(test);

                    _logger.LogInformation($"Email sent! Recipent: {generatedMerchant.Email}");
                    _logger.LogInformation($"Activation Link: {request.BaseUrl}?encodedId={base64String}");

                }
                else 
                {
                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = "Merchant Creation Failed.";
                    response.Data = null;
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Merchant addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
