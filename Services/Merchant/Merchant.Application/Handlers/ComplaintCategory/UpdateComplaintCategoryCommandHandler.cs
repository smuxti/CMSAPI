using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Exceptions;

//using Merchants.Application.Commands.Merchants;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.ComplaintCategory
{
    public class UpdateComplaintCategoryCommandHandler : IRequestHandler<UpdateComplaintCategoryCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaintCategory _ComplaintCategoryRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public UpdateComplaintCategoryCommandHandler(IComplaintCategory merchantRepository, IMapper mapper,
            ILogger<UpdateComplaintCategoryCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
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
       
        public async Task<Response> Handle(UpdateComplaintCategoryCommand request, CancellationToken cancellationToken)
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
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);
                var MerchantToUpdate = await _ComplaintCategoryRepository.GetById(request.ID);
                if (MerchantToUpdate == null)
                {
                    _logger.LogError($"Channel Not found for updation.");
                    throw new MerchantNotFoundException(nameof(MerchantToUpdate), request.ID);
                }

                var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(MerchantToUpdate);
                merchantEntity.UpdatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                //MerchantToUpdate.Type = request.Type ?? MerchantToUpdate.Type;
                MerchantToUpdate.Category = request.Category ?? MerchantToUpdate.Category;
                MerchantToUpdate.Type = request.Type;
                MerchantToUpdate.AltName = request.AltName ?? null;
                //MerchantToUpdate.ResponseTime = request.ResponseTime;
                //MerchantToUpdate.ResponeType = request.ResponeType;




                var generatedMerchant = await _ComplaintCategoryRepository.UpdateAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint Category updated Successfully.";
                response.Data = generatedMerchant;

                _logger.LogInformation($"Category {merchantEntity} added successfully.");

              
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
