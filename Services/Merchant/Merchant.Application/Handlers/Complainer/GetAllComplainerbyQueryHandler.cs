using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complainer;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Complainer
{
    //internal class GetAllComplainerbyQueryHandler
    //{
    //}

    public class GetAllComplainerbyQueryHandler : IRequestHandler<GetAllComplainerQuery, Response>
    {
        private readonly IComplainer _terminalRepository;
        private readonly ILogger<GetAllComplainerbyQueryHandler> _logger;
        private readonly IMapper _mapper;


        public GetAllComplainerbyQueryHandler(IComplainer terminalRepository, IMapper mapper, ILogger<GetAllComplainerbyQueryHandler> logger)
        {
            _mapper = mapper;

            _terminalRepository = terminalRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(GetAllComplainerQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);


                var complaint = await _terminalRepository.GetAllAsync(x => x.isDeleted != true);

                //var complaint = await _terminalRepository.GetAllAsync();

                if (complaint.Count == 0)
                {

                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"Complainer {complaint} not Found.");
                }
                else
                {

                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = "GetAll Complainer  Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"Complainer {complaint}  Successfully.");
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Complainer Find failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }

    }

}
