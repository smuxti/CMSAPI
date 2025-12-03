using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz.Logging;


namespace Merchants.Application.Handlers.Complaint
{
    public class GetMonthlyComplainQueryHandler : IRequestHandler<GetMonthlyComplainQuery, Response>
    {
        private readonly IComplaint _complaint;
        private readonly ILogger<GetMonthlyComplainQueryHandler> _logger;
        private readonly IMapper _mapper;


        public GetMonthlyComplainQueryHandler(IComplaint repo, IMapper mapper, ILogger<GetMonthlyComplainQueryHandler> logger)
        {
            _mapper = mapper;
            _complaint = repo;
            _logger = logger;
        }
        public async Task<Response> Handle(GetMonthlyComplainQuery request,CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Complaint History failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }
    }
}
