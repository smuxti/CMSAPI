using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintDetails;
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
    //internal class GetAllComplaintDetailsHandler
    //{
    //}

    public class GetAllComplaintDetailsHandler : IRequestHandler<GetAllComplaintDetailsQuery, Response>
    {
        private readonly IComplaintDetails _terminalRepository;
        private readonly ILogger<GetAllComplaintDetailsHandler> _logger;
        private readonly IMapper _mapper;


        public GetAllComplaintDetailsHandler(IComplaintDetails terminalRepository, IMapper mapper, ILogger<GetAllComplaintDetailsHandler> logger)
        {
            _mapper = mapper;

            _terminalRepository = terminalRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(GetAllComplaintDetailsQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);

                var complaintCategoriestypes = await _terminalRepository.GetAllAsync(x => x.isDeleted != true);

                //var complaintCategoriestypes = await _terminalRepository.GetAllAsync();
                if (complaintCategoriestypes.Count == 0)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaintCategoriestypes;

                    _logger.LogInformation($"ComplaintDetails {complaintCategoriestypes} not Found.");
                }
                else
                {



                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "GetAll Complaint Detail Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaintCategoriestypes;

                    _logger.LogInformation($"Detail {complaintCategoriestypes}  successfully.");
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Detail addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }

    }
}
