using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.Escalation
{
    public class GetByManagementIdCommandHandler : IRequestHandler<GetByManagementIdCommand, Response>
    {
        private readonly IEscalation _escalationRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetByManagementIdCommandHandler(IEscalation terminalRepository, IMapper mapper, ILogger<GetByManagementIdCommandHandler> logger)
        {
            _mapper = mapper;

            _escalationRepository = terminalRepository;
            _logger = logger;
        }
        public async Task<Response> Handle(GetByManagementIdCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var escalation = await _escalationRepository.GetEscalationByManagementID(request.managementId);

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
                    response.ResponseDescription = "Get Successfully.";
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
