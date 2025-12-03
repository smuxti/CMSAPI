using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Escalation;
using Merchants.Application.Commands.ManagementHierarchy;
using Merchants.Application.Commands.Merchant;
using Merchants.Application.Exceptions;
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
    //internal class DeleteMerchantHandler
    //{
    //}

    public class DeleteMerchantHandler : IRequestHandler<DeleteMerchantCommand, Response>
    {


        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMerchant _IChannelRepository;
        public DeleteMerchantHandler(IMerchant merchantRepository, IMapper mapper, ILogger<DeleteMerchantCommand> logger, IHttpContextAccessor httpContextAccessor)
        {
            _IChannelRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(DeleteMerchantCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();

            try
            {
                var MerchantToBeDeleted = await _IChannelRepository.GetById(request.ID);
                if (MerchantToBeDeleted == null)
                {
                    _logger.LogError($"Merchant Not found for deletion.");
                    throw new MerchantNotFoundException(nameof(MerchantToBeDeleted), request.ID);

                }
                MerchantToBeDeleted.isDeleted = true;
                MerchantToBeDeleted.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                await _IChannelRepository.UpdateAsync(MerchantToBeDeleted);
                _logger.LogInformation($"Merchant {MerchantToBeDeleted} deleted successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Merchant Successfully.";
                //response.Data = generatedMerchant;

                _logger.LogInformation($"Merchant {MerchantToBeDeleted} deleted successfully.");


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Merchant  failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }


    }
}
