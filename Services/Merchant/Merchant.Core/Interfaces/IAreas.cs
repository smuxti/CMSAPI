using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IAreas
    {
        Task<IEnumerable<MerchantLocations>> GetAreaAsyn();
        Task<Merchant> AddAreaAsync(MerchantLocations merchantLocation);

        Task<Merchant> GetAreaByID(int Id);
        Task<bool> DeleteArea(int Id);
    }
}
