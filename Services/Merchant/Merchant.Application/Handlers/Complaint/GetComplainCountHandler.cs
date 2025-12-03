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
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz.Logging;

namespace Merchants.Application.Handlers.Complaint
{
    public class GetComplainCountHandler : IRequestHandler<GetComplainCount, Response>
    {
        private readonly IComplaint _complaint;
        private readonly ILogger<GetComplainCountHandler> _logger;
        private readonly IMapper _mapper;


        public GetComplainCountHandler(IComplaint repo, IMapper mapper, ILogger<GetComplainCountHandler> logger)
        {
            _mapper = mapper;
            _complaint = repo;
            _logger = logger;
        }
        public async Task<Response> Handle(GetComplainCount request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var compcount = await _complaint.GetNoOfComplainToday();
                if (compcount == null)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = compcount;

                    _logger.LogInformation($"Complaint {compcount} not Found.");
                }
                else
                {


                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "GetAll Complaint  Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = compcount;

                    _logger.LogInformation($"Complaint {compcount}  successfully.");
                }

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
