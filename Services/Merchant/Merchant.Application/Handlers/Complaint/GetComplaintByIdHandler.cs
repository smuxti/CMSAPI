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

namespace Merchants.Application.Handlers.Complaint
{
    public class GetComplaintByIdHandler : IRequestHandler<GetComplaintByIdCommand, Response>
    {
        private readonly IComplaint _repo;
        private readonly ILogger<GetComplaintByIdHandler> _logger;
        private readonly IMapper _mapper;

        public GetComplaintByIdHandler(IComplaint repo, IMapper mapper, ILogger<GetComplaintByIdHandler> logger)
        {
            _mapper = mapper;
            _repo = repo;
            _logger = logger;
        }

        public async Task<Response> Handle(GetComplaintByIdCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var complaint = await _repo.GetById(request.Id);

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
