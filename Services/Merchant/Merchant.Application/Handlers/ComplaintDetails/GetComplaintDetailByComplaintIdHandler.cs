using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintDetails;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.ComplaintDetails
{
    public class GetComplaintDetailByComplaintIdHandler : IRequestHandler<GetComplaintDetailByComplaintIdCommand, Response>
    {
        private readonly IComplaintDetails _repo;
        private readonly ILogger<GetComplaintDetailByComplaintIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetComplaintDetailByComplaintIdHandler(IComplaintDetails repo, IMapper mapper, ILogger<GetComplaintDetailByComplaintIdHandler> logger)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
        }

        public async Task<Response> Handle(GetComplaintDetailByComplaintIdCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var complaint = await _repo.GetComplaintDetailByComplaintId(request.ComplaintId);

                if (complaint.Count() == 0)
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
