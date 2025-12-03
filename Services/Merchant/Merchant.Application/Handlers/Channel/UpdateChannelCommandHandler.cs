using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Channel;
using Merchants.Application.Commands.ComplaintCategory;
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

namespace Merchants.Application.Handlers.Channel
{
    //internal class UpdateChannelCommandHandler
    //{
    //}


    public class UpdateChannelCommandHandler : IRequestHandler<UpdateChannelCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IChannel _ChannelRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public UpdateChannelCommandHandler(IChannel merchantRepository, IMapper mapper,
            ILogger<UpdateChannelCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
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

        public async Task<Response> Handle(UpdateChannelCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
              

                var MerchantToUpdate = await _ChannelRepository.GetById(request.Id);
                if (MerchantToUpdate == null)
                {
                    _logger.LogError($"Channel Not found for updation.");
                    throw new MerchantNotFoundException(nameof(MerchantToUpdate), request.Id);
                }

                var merchantEntity = _mapper.Map<Merchants.Core.Entities.Channel>(MerchantToUpdate);
                MerchantToUpdate.ChannelType = request.ChannelType;
                MerchantToUpdate.Remarks = request.Remarks;
                MerchantToUpdate.Status = request.Status;
                MerchantToUpdate.UpdatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);





                var generatedMerchant = await _ChannelRepository.UpdateAsync(merchantEntity);

                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Channel updated Successfully.";
                response.Data = generatedMerchant;

                _logger.LogInformation($"Channel {merchantEntity} added successfully.");


                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Channel addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
