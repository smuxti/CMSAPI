using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Commands.ComplaintCategory;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Complaint
{
    //internal class AddComplaintCommandHandler
    //{
    //}


    public class AddComplaintCommandHandler : IRequestHandler<AddCompaintCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaint _ComplaintCategoryRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public AddComplaintCommandHandler(IComplaint merchantRepository, IMapper mapper,
            ILogger<AddComplaintCommandHandler> logger, IHttpContextAccessor httpContextAccessor
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

        public async Task<Response> Handle(AddCompaintCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
         
                var merchantEntity = _mapper.Map<Merchants.Core.Entities.Complaint>(request);

             
                merchantEntity.CreatedBy = new Guid("00000000-0000-0000-0000-000000000001");  // Static GUID equivalent of '1'
                merchantEntity.ComplaintDate = DateTime.Now;
                merchantEntity.Status = "Active";

                var generatedMerchant = await _ComplaintCategoryRepository.AddAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint  Created Successfully.";
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
