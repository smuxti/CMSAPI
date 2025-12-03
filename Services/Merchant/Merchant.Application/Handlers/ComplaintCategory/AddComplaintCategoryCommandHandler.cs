using AutoMapper;
using EmailManager.MailService;
using MediatR;
using Merchants.Application.Commands.ComplaintCategory;


//using Merchants.Application.Handlers.Merchants;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.ComplaintCategory
{
    public class AddComplaintCategoryCommandHandler:IRequestHandler<AddComplaintCategoryCommand,Response>   
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaintCategory _ComplaintCategoryRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public AddComplaintCategoryCommandHandler(IComplaintCategory merchantRepository, IMapper mapper,
            ILogger<AddComplaintCategoryCommandHandler> logger, IHttpContextAccessor httpContextAccessor
           )
        {
            _ComplaintCategoryRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            //_mail = mail;
            //_configuration = configuration;
            ////_baseUrl = _configuration["Urls:ActivationUrl"];
            //_redisCacheService = redisCacheService;
        }
     
        public async Task<Response> Handle(AddComplaintCategoryCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);

                merchantEntity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);  // Static GUID equivalent of '1'
                merchantEntity.Status = "1";

                var Escalation = "";
                int EscalationId;
                //var existingComplainer = await _ComplaintCategoryRepository.GetAllAsync(x => x.Type == merchantEntity.Type && x.Category == merchantEntity.Category);
                var existingComplainer = await _ComplaintCategoryRepository.GetAllAsync(x => x.Category == merchantEntity.Category);

                var deletedComplainer = existingComplainer.FirstOrDefault(x => x.isDeleted == true);

                if (existingComplainer.Any() && deletedComplainer == null)
                {
                    var firstComplainer = existingComplainer.First();
                    Escalation = firstComplainer.Category ?? string.Empty;
                    EscalationId = firstComplainer.ID;
                    _logger.LogInformation($"Category already exists with ID: {EscalationId}. Using existing Category.");

                    response.Data = new
                    {
                        Message = "Category already exists.",
                        EscalationDetails = Escalation
                    };
                }
                else
                {
                    response.ResponseDescription = "Category Created Successfully.";
                    var generatedMerchant = await _ComplaintCategoryRepository.AddAsync(merchantEntity);
                    EscalationId = generatedMerchant.ID;
                    Escalation = generatedMerchant.Category ?? string.Empty;
                    response.Data = generatedMerchant;
                    _logger.LogInformation($"New Category added with ID: {EscalationId}.");
                }

                response.isSuccess = true;
                response.ResponseCode = 1;
           
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Category addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }

}
