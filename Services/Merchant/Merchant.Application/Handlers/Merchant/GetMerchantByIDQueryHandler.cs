using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Application.Commands.Merchant;
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

namespace Merchants.Application.Handlers.Merchant
{
    public class GetMerchantByIDQueryHandler : IRequestHandler<GetMerchantByIDQuery, Response>
    {
        private readonly IMerchant _terminalRepository;
        private readonly ILogger<GetMerchantByIDQueryHandler> _logger;
        private readonly IMapper _mapper;


        public GetMerchantByIDQueryHandler(IMerchant terminalRepository, IMapper mapper, ILogger<GetMerchantByIDQueryHandler> logger)
        {
            _mapper = mapper;

            _terminalRepository = terminalRepository;
            _logger = logger;
        }

        public async Task<Response> Handle(GetMerchantByIDQuery request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                //var merchantEntity = _mapper.Map<Merchants.Core.Entities.ComplaintCategory>(request);

                var complaint = await _terminalRepository.GetMerchantByID(request.Id);


                //var complaint = await _terminalRepository.GetAllAsync();
                if (complaint == null)
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
