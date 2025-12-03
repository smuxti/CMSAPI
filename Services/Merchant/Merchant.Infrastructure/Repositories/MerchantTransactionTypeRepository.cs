using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Merchants.Infrastructure.Repositories
{
    internal class MerchantTransactionTypeRepository : AsyncRepository<MerchantTransactionType>, IMerchantTransactionTypeRepository
    {
        public MerchantTransactionTypeRepository(MerchantContext merchantContext, ILogger<MerchantTransactionTypeRepository> logger) : base(merchantContext, logger)
        {
            
        }
        public async Task<IEnumerable<MerchantTransactionType>> GetByMerchantID(Guid MerchantID)
        {
            return await _dbContext.merchantTransactionTypes.Where(o => o.MerchantID.Equals(MerchantID)).ToListAsync();
        }
    }
}
