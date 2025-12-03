using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Merchants;
using Merchants.Application.Exceptions;
using Merchants.Application.Responses;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.MerchantHandlers
{
    internal class UpdateMerchantCommandHandler : IRequestHandler<UpdateMerchantCommand, Response>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IRedisCacheService _redisCacheService;

        public UpdateMerchantCommandHandler(IMerchantRepository merchantRepository, IMapper mapper, 
            ILogger<UpdateMerchantCommandHandler> logger, IHttpContextAccessor httpContextAccessor, IRedisCacheService redisCacheService)
        {
            _merchantRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _redisCacheService = redisCacheService;
        }
        public async Task<Response> Handle(UpdateMerchantCommand request, CancellationToken cancellationToken)
        {
            Response response = new Response();
            try
            {
                var MerchantToUpdate = await _merchantRepository.GetById(request.Id);
                if (MerchantToUpdate == null)
                {
                    _logger.LogError($"Merchant Not found for updation.");
                    throw new MerchantNotFoundException(nameof(MerchantToUpdate), request.Id);
                }

                _mapper.Map(request, MerchantToUpdate, typeof(UpdateMerchantCommand), typeof(Merchant));
                MerchantToUpdate.UpdatedBy = Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value, out var parsedGuid)
                    ? parsedGuid
                    : (Guid?)null;
                var UpdatedMerchant = await _merchantRepository.UpdateAsync(MerchantToUpdate);
                _logger.LogInformation($"Merchant {MerchantToUpdate} updated successfully.");
                response.isSuccess = true;
                response.ResponseCode = 1;
                response.ResponseDescription = "Merchant Update Successfully.";
                response.Data = request;

                var account = await _redisCacheService?.GetCacheValueAsynca($"Id:{UpdatedMerchant.Id}");
                if (account is not null)
                {
                    var deleteKey = await _redisCacheService.DeleteCacheValueAsync($"Id:{UpdatedMerchant.Id}");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Merchant updation failed {ex.Message}.");
                response.isSuccess = false;
                response.ResponseCode = 0;
                response.ResponseDescription = "Merchant Update Failed.";
                response.Data = request;
                return response;
            }

        }
    }
}
