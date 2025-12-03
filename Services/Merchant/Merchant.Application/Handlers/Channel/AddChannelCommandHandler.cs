using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Channel;
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

namespace Merchants.Application.Handlers.Channel
{

    public class AddChannelCommandHandler : IRequestHandler<AddChannelCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IChannel _ChannelRepository;
        //private readonly Mail _mail;
        //private readonly IConfiguration _configuration;
        //private readonly string _baseUrl;
        //private readonly IRedisCacheService _redisCacheService;


        public AddChannelCommandHandler(IChannel merchantRepository, IMapper mapper,
            ILogger<AddChannelCommandHandler> logger, IHttpContextAccessor httpContextAccessor
           )
        {
            _ChannelRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> Handle(AddChannelCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                
                var channel = _mapper.Map<Merchants.Core.Entities.Channel>(request);

    
                channel.CreatedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value); //new Guid("00000000-0000-0000-0000-000000000001");  // Static GUID equivalent of '1'
                channel.Status = "Active";



                var Escalation = "";
                int EscalationId;
                //var existingComplainer = await _ChannelRepository.GetAllAsync(x => x.ChannelType == merchantEntity.ChannelType || x.isDeleted == true);
                var existingComplainer = await _ChannelRepository.GetAllAsync(x => x.ChannelType == channel.ChannelType);

                var deletedComplainer = existingComplainer.FirstOrDefault(x => x.isDeleted == true);

                if (existingComplainer.Any() && deletedComplainer == null)
                {
                    var firstComplainer = existingComplainer.First();
                    Escalation = firstComplainer.ChannelType ?? string.Empty;
                    EscalationId = firstComplainer.ID;
                    _logger.LogInformation($"Channel Type already exists with ID: {EscalationId}. Using existing complainer.");


                    response.Data = new
                    {
                        Message = "Channel Type already exists.",
                        EscalationDetails = Escalation
                    };
                }
                else
                {
                    response.ResponseDescription = "Channel Created Successfully.";
                    var generatedMerchant = await _ChannelRepository.AddAsync(channel);
                    EscalationId = generatedMerchant.ID;
                    Escalation = generatedMerchant.ChannelType ?? string.Empty;
                    response.Data = generatedMerchant;
                    _logger.LogInformation($"New Channel added with ID: {EscalationId}.");
                }




                response.isSuccess = true;
                response.ResponseCode = 1;
      

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
