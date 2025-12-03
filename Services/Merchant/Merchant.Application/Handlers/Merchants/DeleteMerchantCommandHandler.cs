using AutoMapper;
using MediatR;
using Merchants.Application.Commands.Merchants;
using Merchants.Application.Exceptions;
using Merchants.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Merchants.Application.Handlers.MerchantHandlers
{
    internal class DeleteMerchantCommandHandler : IRequestHandler<DeleteMerchantCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IMerchantRepository _merchantRepository;
        public DeleteMerchantCommandHandler(IMerchantRepository merchantRepository, IMapper mapper, ILogger<DeleteMerchantCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _merchantRepository = merchantRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Handle(DeleteMerchantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var MerchantToBeDeleted = await _merchantRepository.GetById(request.Id);
                if (MerchantToBeDeleted == null)
                {
                    _logger.LogError($"Merchant Not found for deletion.");
                    throw new MerchantNotFoundException(nameof(MerchantToBeDeleted), request.Id);

                }
                MerchantToBeDeleted.DeletedBy= Guid.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("UserID")?.Value);
                await _merchantRepository.DeleteAsync(MerchantToBeDeleted);
                _logger.LogInformation($"Merchant {MerchantToBeDeleted} deleted successfully.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Merchant deletion failed {ex.Message}.");
                return false;
            }
        }
    }
}
