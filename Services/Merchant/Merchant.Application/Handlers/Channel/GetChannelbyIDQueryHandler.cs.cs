using AutoMapper;
using MediatR;
using Merchants.Application.Handlers.Complaint;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Channel
{
    public class GetChannelbyIDQueryHandler : IRequestHandler<GetChannelByIDQuery, Response>
    {
        private readonly IChannel _channelRepository;
        private readonly ILogger<GetChannelbyIDQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetChannelbyIDQueryHandler(IChannel channelRepository, IMapper mapper, ILogger<GetChannelbyIDQueryHandler> logger)
        {
            _mapper = mapper;

            _channelRepository = channelRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(GetChannelByIDQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);


                //var complaint = await _escalationRepository.GetAllAsync(x => x.CategoryID == request.CategoryID && x.Type == request.Type);
                var escalation = await _channelRepository.GetCHANELByID(request.ID);

                //var complaint = await _terminalRepository.GetAllAsync();


                if (escalation == null)
                {

                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = null;

                    _logger.LogInformation($"Channel {escalation} not Found.");
                }
                else
                {
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " GetChannelByID Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = escalation;

                    _logger.LogInformation($"GetChannel {escalation}  successfully.");
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetChannelByID addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }


    }
}
