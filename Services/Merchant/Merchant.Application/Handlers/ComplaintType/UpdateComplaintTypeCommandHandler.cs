using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Commands.ComplaintType;
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

namespace Merchants.Application.Handlers.ComplaintType
{
    //internal class UpdateComplaintTypeCommandHandler
    //{
    //}


    public class UpdateComplaintTypeCommandHandler : IRequestHandler<UpdateCompaintTypeCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaintType _ComplaintCategoryRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public UpdateComplaintTypeCommandHandler(IComplaintType merchantRepository, IMapper mapper,
            ILogger<UpdateComplaintTypeCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
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

        public async Task<Response> Handle(UpdateCompaintTypeCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
        
                var MerchantToUpdate = await _ComplaintCategoryRepository.GetById(request.ID);
                if (MerchantToUpdate == null)
                {
                    _logger.LogError($"Channel Not found for updation.");
                    throw new MerchantNotFoundException(nameof(MerchantToUpdate), request.ID);
                }

                var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintType>(MerchantToUpdate);



                MerchantToUpdate.ID = request.ID;
                MerchantToUpdate.ComplaintTypes = request.ComplaintTypes ?? MerchantToUpdate.ComplaintTypes;
                

         
             var generatedMerchant = await _ComplaintCategoryRepository.UpdateAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint Type updated Successfully.";
                response.Data = generatedMerchant;

                _logger.LogInformation($"Complaint {merchantEntity} added successfully.");


                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Complaint addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
