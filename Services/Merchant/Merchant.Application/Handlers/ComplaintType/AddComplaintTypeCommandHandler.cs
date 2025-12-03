using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
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
    //internal class AddComplaintTypeCommandHandler
    //{
    //}


    public class AddComplaintTypeCommandHandler : IRequestHandler<AddCompaintTypeCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaintType _ComplaintCategoryRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public AddComplaintTypeCommandHandler(IComplaintType merchantRepository, IMapper mapper,
            ILogger<AddComplaintTypeCommandHandler> logger, IHttpContextAccessor httpContextAccessor )
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

        public async Task<Response> Handle(AddCompaintTypeCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
               
                var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintType>(request);

               
                merchantEntity.CreatedBy = new Guid("00000000-0000-0000-0000-000000000001");  // Static GUID equivalent of '1'
         
                //merchantEntity.Status = "aCTI";
                var ComplaintTypes = "";
                int ComplaintID = 0;

                var existingComplainer = await _ComplaintCategoryRepository.GetAllAsync(x => x.ComplaintTypes == merchantEntity.ComplaintTypes);

                var deletedComplainer = existingComplainer.FirstOrDefault(x => x.isDeleted == true);

                if (existingComplainer.Any() && deletedComplainer == null)
                {
                    var firstComplainer = existingComplainer.First();
                    ComplaintTypes = firstComplainer.ComplaintTypes ?? string.Empty;
                    ComplaintID = firstComplainer.ID;
                    _logger.LogInformation($"Category Types already exists with ID: {ComplaintTypes}. Using existing Category.");


                    response.Data = new
                    {
                        Message = "Category Types already exists.",
                        EscalationDetails = ComplaintTypes
                    };
                }
                else
                {
                    response.ResponseDescription = "Category Types Created Successfully.";
                    var generatedMerchant = await _ComplaintCategoryRepository.AddAsync(merchantEntity);
                    ComplaintID = generatedMerchant.ID;
                    ComplaintTypes = generatedMerchant.ComplaintTypes ?? string.Empty;
                    response.Data = generatedMerchant;
                    _logger.LogInformation($"New Category Types added with ID: {ComplaintTypes}.");
                }




                //var generatedMerchant = await _ComplaintCategoryRepository.AddAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                //response.ResponseDescription = "Complaint Type Created Successfully.";
                //response.Data = generatedMerchant;

                //_logger.LogInformation($"Type {merchantEntity} added successfully.");


                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Type addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }

}
