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
    public class GetEscalationByIDCommandHandler : IRequestHandler<GetEscalationByIDQuery, Response>
    {
        private readonly IEscalation _escalationRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public GetEscalationByIDCommandHandler(IEscalation terminalRepository, IMapper mapper, ILogger<GetEscalationByIDCommandHandler> logger)
        {
            _mapper = mapper;

            _escalationRepository = terminalRepository;
            _logger = logger;
        }
        public async Task<Response> Handle(GetEscalationByIDQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);


                //var complaint = await _escalationRepository.GetAllAsync(x => x.CategoryID == request.CategoryID && x.Type == request.Type);
                var escalation = await _escalationRepository.GetEscalationByID(request.MatrixID);

                //var complaint = await _terminalRepository.GetAllAsync();


                if (escalation == null)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = null;

                    _logger.LogInformation($"Escalation {escalation} not Found.");
                }
                else
                {
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " GetEscalatonByID Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = escalation;

                    _logger.LogInformation($"GetEscalation {escalation}  successfully.");
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
