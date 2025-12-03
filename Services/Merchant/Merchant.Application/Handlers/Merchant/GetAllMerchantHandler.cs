using AutoMapper;
using MediatR;
using Merchants.Application.Queries;
using Merchants.Application.Responses;
using Merchants.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Handlers.Merchant
{
    //internal class GetAllMerchantHandler
    //{
    //}

    public class GetAllMerchantHandler : IRequestHandler<GetAllMerchantQuery, Response>
    {
        private readonly IMerchant _terminalRepository;
        private readonly ILogger<GetAllMerchantHandler> _logger;
        private readonly IMapper _mapper;


        public GetAllMerchantHandler(IMerchant terminalRepository, IMapper mapper, ILogger<GetAllMerchantHandler> logger)
        {
            _mapper = mapper;

            _terminalRepository = terminalRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(GetAllMerchantQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);

                var complaint = await _terminalRepository.GetAllAsync(x => x.isDeleted != true);


                //var complaint = await _terminalRepository.GetAllAsync();
                if (complaint.Count == 0)
                {

                    response.isSuccess = false;
                    response.ResponseCode = 0;
                    response.ResponseDescription = " Record not Found.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"Merchants {complaint} not Found.");
                }
                else
                {


                    response.isSuccess = true;
                    response.ResponseCode = 1;
                    response.ResponseDescription = " GetAllMerchant  Successfully.";
                    //response.Data = _mapper.Map<List<MerchantResponse>>(tenants); 
                    response.Data = complaint;

                    _logger.LogInformation($"Merchant {complaint}  successfully.");
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Merchant Get failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }


        }

    }
}
