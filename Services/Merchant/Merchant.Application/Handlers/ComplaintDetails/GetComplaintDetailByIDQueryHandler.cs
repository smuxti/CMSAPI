using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complaint;
using Merchants.Application.Commands.ComplaintDetails;
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

namespace Merchants.Application.Handlers.ComplaintDetails
{
    public class GetComplaintDetailByIDQueryHandler : IRequestHandler<GetComplaintDetailByIDQuery, Response>
    {
        private readonly IComplaintDetails _repo;
        private readonly ILogger<GetComplaintDetailByIDQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetComplaintDetailByIDQueryHandler(IComplaintDetails repo, IMapper mapper, ILogger<GetComplaintDetailByIDQueryHandler> logger)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
        }

        public async Task<Response> Handle(GetComplaintDetailByIDQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var complaint = await _repo.GetComplaintDetailByID(request.ID);

                if (complaint == null)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = "Record not Found.";
                    response.Data = null;

                    _logger.LogInformation($"Complaint {complaint} not Found.");
                    return response;
                }

                if (complaint.isDeleted == true)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = "Record not Found.";
                    response.Data = null;

                    _logger.LogInformation($"Complaint {complaint} not Found.");
                    return response;
                }


                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Get Complaint Successful.";
                response.Data = complaint;

                _logger.LogInformation($"Complaint {complaint} found successfully.");
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Complaint addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }

    }
}
