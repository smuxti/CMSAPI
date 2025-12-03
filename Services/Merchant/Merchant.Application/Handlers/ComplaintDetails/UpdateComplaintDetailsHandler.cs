using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Commands.ComplaintDetails;
using Merchants.Application.Exceptions;
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
    //internal class UpdateComplaintDetailsHandler
    //{
    //}

    public class UpdateComplaintDetailsHandler : IRequestHandler<UpdateCompaintDetailsCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaintDetails _ComplaintCategoryRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public UpdateComplaintDetailsHandler(IComplaintDetails merchantRepository, IMapper mapper,
            ILogger<UpdateComplaintDetailsHandler> logger, IHttpContextAccessor httpContextAccessor)
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

        public async Task<Response> Handle(UpdateCompaintDetailsCommand request, CancellationToken cancellationToken)
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
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintDetails>(request);
                var MerchantToUpdate = await _ComplaintCategoryRepository.GetById(request.ID);
                if (MerchantToUpdate == null)
                {
                    _logger.LogError($"Channel Not found for updation.");
                    throw new MerchantNotFoundException(nameof(MerchantToUpdate), request.ID);
                }

                var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintDetails>(MerchantToUpdate);

                merchantEntity.UpdatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                //MerchantToUpdate.ComplainerID = request.ComplainerID ?? MerchantToUpdate.ComplainerID;
                //MerchantToUpdate.ComplaintDetail = request.ComplaintDetail ?? MerchantToUpdate.ComplaintDetail;
                MerchantToUpdate.Description = request.Description ?? MerchantToUpdate.Description;
                //MerchantToUpdate.AssignedTo = request.AssignedTo;
                MerchantToUpdate.Remarks = request.Remarks ?? MerchantToUpdate.Remarks;
                MerchantToUpdate.CurrentStatus = request.CurrentStatus;


 
        //merchantEntity.Posted = 0;
        //merchantEntity.ReasonCode = "Created";
        //merchantEntity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
        //merchantEntity.CreatedAt = DateTime.Now.ToLocalTime();
        //merchantEntity.CreatedBy = new Guid();
        //merchantEntity.CreatedBy = new Guid("00000000-0000-0000-0000-000000000001");  // Static GUID equivalent of '1'
        //merchantEntity.CreatedAt = DateTime.Now.ToLocalTime();
        //merchantEntity.Status = "1";

        var generatedMerchant = await _ComplaintCategoryRepository.UpdateAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint Details updated Successfully.";
                response.Data = generatedMerchant;

                _logger.LogInformation($"ComplaintDetails {merchantEntity} added successfully.");


                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"ComplaintDetails addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
