using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Complainer;
using Merchants.Application.Commands.Complaint;
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

namespace Merchants.Application.Handlers.Complainer
{
    //internal class DeleteComplainerCommandHandler
    //{
    //}

    public class DeleteComplainerCommandHandler : IRequestHandler<DeleteComplainerCommand, Response>
    {


        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplainer _IChannelRepository;
        public DeleteComplainerCommandHandler(IComplainer merchantRepository, IMapper mapper, ILogger<DeleteComplainerCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _IChannelRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(DeleteComplainerCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();

            try
            {
                var MerchantToBeDeleted = await _IChannelRepository.GetById(request.ID);
                if (MerchantToBeDeleted == null)
                {
                    _logger.LogError($"Complainer Not found for deletion.");
                    throw new MerchantNotFoundException(nameof(MerchantToBeDeleted), request.ID);

                }
                //MerchantToBeDeleted.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                await _IChannelRepository.DeleteAsync(MerchantToBeDeleted);
                _logger.LogInformation($"Complainer {MerchantToBeDeleted} deleted successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complainer deleted Successfully.";
                //response.Data = generatedMerchant;

                _logger.LogInformation($"Complainer {MerchantToBeDeleted} deleted successfully.");


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Complainer addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }

    }

}
