using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Commands.ComplaintDetails;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.ComplaintDetails
{
    //internal class AddComplaintDetailsHandler
    //{

    //}

    public class AddComplaintDetailsHandler : IRequestHandler<AddCompaintDetailsCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaintDetails _ComplaintCategoryRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public AddComplaintDetailsHandler(IComplaintDetails merchantRepository, IMapper mapper,
            ILogger<AddComplaintDetailsHandler> logger, IHttpContextAccessor httpContextAccessor
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

        public async Task<Response> Handle(AddCompaintDetailsCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var checkmerchant = await _ComplaintCategoryRepository.AddComplaintCategoryAsync(request.);
                //if (checkmerchant != null)
                //{
                //    response.isSuccess = false;
                //    response.ResponseCode = 0;
                //    response.ResponseDescription = "Merchant Code Already Exists.";
                //    response.Data = null;
                //    return response;
                //}
                //var merchantEntity = _mapper.Map<ComplaintCategory>(request);
                var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintDetails>(request);

                //merchantEntity.Posted = 0;
                //merchantEntity.ReasonCode = "Created";
                //merchantEntity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                //merchantEntity.CreatedAt = DateTime.Now.ToLocalTime();
                //merchantEntity.CreatedBy = new Guid();
                merchantEntity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);  // Static GUID equivalent of '1'
                //merchantEntity.CreatedAt = DateTime.Now.ToLocalTime();
                //merchantEntity.ComplaintDate = DateTime.Now;
                merchantEntity.Status = "Active";


                var generatedMerchant = await _ComplaintCategoryRepository.AddAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint Detail  Created Successfully.";
                response.Data = generatedMerchant;

                _logger.LogInformation($"Complaint {merchantEntity} added successfully.");


                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Complaint Detail addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
