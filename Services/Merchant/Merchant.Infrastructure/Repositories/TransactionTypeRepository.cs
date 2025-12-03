using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Merchants.Infrastructure.Repositories
{
    internal class TransactionTypeRepository: AsyncRepository<TransactionType>, ITransactionTypeRepository
    {
        public TransactionTypeRepository(MerchantContext merchantContext, ILogger<TransactionTypeRepository> logger) : base(merchantContext, logger)
        {
            
        }

    }
}
