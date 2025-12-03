using AutoMapper;
using MediatR;
using Merchants.Application.Commands.ComplaintType;
using Merchants.Application.Commands.MerchantLocation;
using Merchants.Application.Exceptions;
using Merchants.Application.Handlers.ComplaintType;
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

namespace Merchants.Application.Handlers.MerchantLocation
{
    public class DeleteMerchantLocationHandler : IRequestHandler<DeleteMerchantLocationCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IZones _zoneRepository;
        public DeleteMerchantLocationHandler(IZones zoneRepository, IMapper mapper, ILogger<DeleteComplaintTypeCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _zoneRepository = zoneRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> Handle(DeleteMerchantLocationCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();

            try
            {
                var MerchantToBeDeleted = await _zoneRepository.GetById(request.ID);
                if (MerchantToBeDeleted == null)
                {
                    _logger.LogError($"Locaton Not found for deletion.");
                    throw new MerchantNotFoundException(nameof(MerchantToBeDeleted), request.ID);

                }
                //MerchantToBeDeleted.DeletedBy = Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                await _zoneRepository.DeleteAsync(MerchantToBeDeleted);
                _logger.LogInformation($"Location {MerchantToBeDeleted} deleted successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Record deleted Successfully.";
                //response.Data = generatedMerchant;

                _logger.LogInformation($"Type {MerchantToBeDeleted} deleted successfully.");


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Type addition failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = ex.Message;
                response.Data = null;
                return response;
            }
        }

    }
}
