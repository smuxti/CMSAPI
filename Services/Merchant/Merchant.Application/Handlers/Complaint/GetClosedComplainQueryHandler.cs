using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Complaint
{
    public class GetClosedComplainQueryHandler : IRequestHandler<GetClosedComplain, Response>
    {
        private readonly IComplaint _complaint;
        private readonly ILogger<GetClosedComplainQueryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public GetClosedComplainQueryHandler(IComplaint repo, IMapper mapper, ILogger<GetClosedComplainQueryHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _complaint = repo;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(GetClosedComplain request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var userId = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);

                var compcount = await _complaint.GetTotalClosedComplain();
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
