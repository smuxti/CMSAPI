using Merchants.Core.Entities;

namespace Merchants.Core.Interfaces
{
    public interface IMerchantTransactionTypeRepository:IAsyncRepository<MerchantTransactionType>
    {
        Task<IEnumerable<MerchantTransactionType>> GetByMerchantID(Guid MerchantID);
    }
}
