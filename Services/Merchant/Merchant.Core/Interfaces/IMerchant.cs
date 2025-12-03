using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IMerchant:IAsyncRepository<Merchant>
    {
        Task<IEnumerable<Merchant>> GetMerchantsAsyn();
        Task<Merchant> AddMerchantAsync(Merchant merchant);

        Task<Merchant> GetMerchantByID(int ID);
        Task<string> GetMerchantCode();
        Task<bool> DeleteMerchant(int Id);
        Task<Merchant> GetMerchantByZone(int ID);
        Task<Merchant> GetMerchantByArea(int ID);

    }
}
