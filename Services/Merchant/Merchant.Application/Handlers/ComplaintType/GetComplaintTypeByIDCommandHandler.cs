using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.ComplaintType
{
    public class GetComplaintTypeByIDCommandHandler : IRequestHandler<GetComplaintTypeByIDQuery, Response>
    {
        private readonly IComplaintType _terminalRepository;
        private readonly ILogger<GetComplaintTypeByIDCommandHandler> _logger;
        private readonly IMapper _mapper;

        public GetComplaintTypeByIDCommandHandler(IComplaintType terminalRepository, IMapper mapper, ILogger<GetComplaintTypeByIDCommandHandler> logger)
        {
            _mapper = mapper;

            _terminalRepository = terminalRepository;
            _logger = logger;
        }
        public async Task<Response> Handle(GetComplaintTypeByIDQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);


                //var complaint = await _escalationRepository.GetAllAsync(x => x.CategoryID == request.CategoryID && x.Type == request.Type);
                var escalation = await _terminalRepository.GetType(request.ID);

                //var complaint = await _terminalRepository.GetAllAsync();


                if (escalation == null)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = null;

                    _logger.LogInformation($"Complaint Type {escalation} not Found.");
                }
                else
                {
                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " GetComplaintTypeByID Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = escalation;

                    _logger.LogInformation($"GetComplaintTypeByID {escalation}  successfully.");
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetComplaintTypeByID get failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }


    }

}
