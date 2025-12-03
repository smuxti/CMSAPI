using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complainer;
using Merchants.Application.Commands.Complaint;
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

namespace Merchants.Application.Handlers.Complainer
{
    //internal class UpdateComplainerCommandHandler
    //{
    //}

    public class UpdateComplainerCommandHandler : IRequestHandler<UpdateComplainerCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplainer _ChannelRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public UpdateComplainerCommandHandler(IComplainer merchantRepository, IMapper mapper,
            ILogger<UpdateComplainerCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _ChannelRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            //_mail = mail;
            //_configuration = configuration;
            ////_baseUrl = _configuration["Urls:ActivationUrl"];
            //_redisCacheService = redisCacheService;
        }

        public async Task<Response> Handle(UpdateComplainerCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
            
                var MerchantToUpdate = await _ChannelRepository.GetById(request.ID);
                if (MerchantToUpdate == null)
                {
                    _logger.LogError($"Complainer Not found for updation.");
                    throw new MerchantNotFoundException(nameof(MerchantToUpdate), request.ID);
                }

                var merchantEntity = _mapper.Map<Merchants.Core.Entities.Complainer>(MerchantToUpdate);
                MerchantToUpdate.ID = request.ID;
                MerchantToUpdate.Name = request.Name ?? MerchantToUpdate.Name;
                MerchantToUpdate.Mobile = request.Mobile ?? MerchantToUpdate.Mobile;
                MerchantToUpdate.Email = request.Email ?? MerchantToUpdate.Mobile;
                MerchantToUpdate.Remarks = request.Remarks ?? MerchantToUpdate.Remarks; 
                MerchantToUpdate.UpdatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);



                var generatedMerchant = await _ChannelRepository.UpdateAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complainer updated Successfully.";
                response.Data = generatedMerchant;

                _logger.LogInformation($"Channel {merchantEntity} added successfully.");


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
