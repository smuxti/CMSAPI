using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Application.Commands.Merchant;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Merchant
{
    //internal class AddMerchantHandler
    //{
    //}

    public class AddMerchantHandler : IRequestHandler<AddMerchantCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMerchant _ComplaintCategoryRepository;
        private readonly IZones  _merchantlocation;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public AddMerchantHandler(IMerchant merchantRepository, IMapper mapper,
            ILogger<AddMerchantHandler> logger, IHttpContextAccessor httpContextAccessor, IZones merchantlocation)
        {
            _ComplaintCategoryRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _merchantlocation = merchantlocation;

            //_mail = mail;
            //_configuration = configuration;
            ////_baseUrl = _configuration["Urls:ActivationUrl"];
            //_redisCacheService = redisCacheService;
        }

        public async Task<Response> Handle(AddMerchantCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var merchantEntity = _mapper.Map<Merchants.Core.Entities.Merchant>(request);
                Random random = new Random();
                    
                int randomNumber = random.Next(10000, 99999);
                string code = "2510" + randomNumber.ToString();

                merchantEntity.MerchantCode = code;
                merchantEntity.MerchantCode = await _ComplaintCategoryRepository.GetMerchantCode();
                //merchantEntity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
               
                merchantEntity.Status = "Active";

                var Escalation = "";
                int EscalationId;
                var existingComplainer = await _ComplaintCategoryRepository.GetAllAsync(x => x.MerchantCode == merchantEntity.MerchantCode && x.MerchantName == merchantEntity.MerchantName);


                var deletedComplainer = existingComplainer.FirstOrDefault(x => x.isDeleted == true);

                if (existingComplainer.Any() && deletedComplainer == null)
                {
                    var firstComplainer = existingComplainer.First();
                    Escalation = firstComplainer.MerchantName ?? string.Empty;
                    EscalationId = firstComplainer.ID;
                    _logger.LogInformation($"Merchant Type already exists with ID: {EscalationId}. Using existing Merchant.");


                    response.Data = new
                    {
                        Message = "Merchant already exists.",
                        EscalationDetails = Escalation
                    };
                }
                else
                {
                    response.ResponseDescription = "Merchant Created Successfully.";
                    var generatedMerchant = await _ComplaintCategoryRepository.AddAsync(merchantEntity);
                    EscalationId = generatedMerchant.ID;
                    Escalation = generatedMerchant.MerchantName ?? string.Empty;
                    response.Data = generatedMerchant;
                    _logger.LogInformation($"New Merchant added with ID: {EscalationId}.");
                }

                response.isSuccess = true;
                response.ResponseCode = 1;


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
