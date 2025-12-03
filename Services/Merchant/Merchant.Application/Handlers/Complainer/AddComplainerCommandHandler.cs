using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complainer;
using Merchants.Application.Commands.Complaint;
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

namespace Merchants.Application.Handlers.Complainer
{
    //internal class AddComplainerCommandHandler
    //{
    //}
    public class AddComplainerCommandHandler : IRequestHandler<AddComplainerCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplainer _ComplaintCategoryRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public AddComplainerCommandHandler(IComplainer merchantRepository, IMapper mapper,
            ILogger<AddComplainerCommandHandler> logger, IHttpContextAccessor httpContextAccessor
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

        public async Task<Response> Handle(AddComplainerCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
        
                var merchantEntity = _mapper.Map<Merchants.Core.Entities.Complainer>(request);

          
                merchantEntity.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);  // Static GUID equivalent of '1'

                merchantEntity.Status = "Active";


                var existingComplainer = await _ComplaintCategoryRepository.GetAllAsync(x => x.Mobile == merchantEntity.Mobile || x.Email == merchantEntity.Email);
                var deletedComplainer = existingComplainer.FirstOrDefault(x => x.isDeleted == true);

                if (existingComplainer.Any() && deletedComplainer == null)
                {
                

                   var complainerId = existingComplainer.FirstOrDefault().ID;
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "Complainer already exists with ID: {complainerId}. Using existing complainer.";
                    response.Data = merchantEntity;
                    _logger.LogInformation($"Complainer already exists with ID: {complainerId}. Using existing complainer.");
                }
                else
                {
                    var generatedMerchant = await _ComplaintCategoryRepository.AddAsync(merchantEntity);
                    var complainerId = existingComplainer.FirstOrDefault().ID;
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "Complainer  Created Successfully.";
                    response.Data = generatedMerchant;

                    _logger.LogInformation($"New complainer added with ID: {complainerId}.");
                }


                //var generatedMerchant = await _ComplaintCategoryRepository.AddAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
         



                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Complainer addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
