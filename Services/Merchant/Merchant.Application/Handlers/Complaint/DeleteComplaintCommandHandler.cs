using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Channel;
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

namespace Merchants.Application.Handlers.Complaint
{

    public class DeleteComplaintCommandHandler : IRequestHandler<DeleteCompaintCommand, Response>
    {


        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IComplaint _IChannelRepository;
        public DeleteComplaintCommandHandler(IComplaint merchantRepository, IMapper mapper, ILogger<DeleteComplaintCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _IChannelRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(DeleteCompaintCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();

            try
            {
                var MerchantToBeDeleted = await _IChannelRepository.GetById(request.ID);
                if (MerchantToBeDeleted == null)
                {
                    _logger.LogError($"Complaint Not found for deletion.");
                    throw new MerchantNotFoundException(nameof(MerchantToBeDeleted), request.ID);

                }
                //MerchantToBeDeleted.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                MerchantToBeDeleted.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                await _IChannelRepository.DeleteAsync(MerchantToBeDeleted);
                _logger.LogInformation($"Complaint {MerchantToBeDeleted} deleted successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Complaint deleted Successfully.";
                //response.Data = generatedMerchant;

                _logger.LogInformation($"Complaint {MerchantToBeDeleted} deleted successfully.");


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
