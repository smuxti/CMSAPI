using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Escalation
{
    //internal class GetAllEscalationCommandHandler
    //{
    //}

    public class GetAllEscalationCommandHandler : IRequestHandler<GetAllEscalationQuery, Response>
    {
        private readonly IEscalation _terminalRepository;
        private readonly ILogger<GetAllEscalationCommandHandler> _logger;
        private readonly IMapper _mapper;


        public GetAllEscalationCommandHandler(IEscalation terminalRepository, IMapper mapper, ILogger<GetAllEscalationCommandHandler> logger)
        {
            _mapper = mapper;

            _terminalRepository = terminalRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(GetAllEscalationQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);


                var complaint = await _terminalRepository.GetEscalationAsyn(); //.GetAllAsync(x => x.isDeleted != true);

                //var complaint = await _terminalRepository.GetAllAsync();


                if (complaint  == null)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"Escalation {complaint} not Found.");
                }
                else
                {
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " GetAllEscalation Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"GetEscalation {complaint}  successfully.");
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllEscalation addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }

    }
}
