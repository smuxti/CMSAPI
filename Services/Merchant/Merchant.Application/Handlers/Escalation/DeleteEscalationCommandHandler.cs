using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Commands.Escalation;
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

namespace Merchants.Application.Handlers.Escalation
{
    //internal class DeleteEscalationCommandHandler
    //{
    //}

    public class DeleteEscalationCommandHandler : IRequestHandler<DeleteEscalationCommand, Response>
    {


        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IEscalation _IChannelRepository;
        public DeleteEscalationCommandHandler(IEscalation merchantRepository, IMapper mapper, ILogger<DeleteEscalationCommand> logger, IHttpContextAccessor httpContextAccessor)
        {
            _IChannelRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(DeleteEscalationCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();

            try
            {
                var MerchantToBeDeleted = await _IChannelRepository.GetById(request.MatrixID);
                if (MerchantToBeDeleted == null)
                {
                    _logger.LogError($"Escalation Not found for deletion.");
                    throw new MerchantNotFoundException(nameof(MerchantToBeDeleted), request.MatrixID);

                }
                MerchantToBeDeleted.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                await _IChannelRepository.DeleteAsync(MerchantToBeDeleted);
                _logger.LogInformation($"Escalation {MerchantToBeDeleted} deleted successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Escalation Category deleted Successfully.";
                //response.Data = generatedMerchant;

                _logger.LogInformation($"Escalation {MerchantToBeDeleted} deleted successfully.");


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Escalation addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }

      
    }
}
