using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Core.Entities;

namespace Merchants.Core.Interfaces
{
    public interface IEquipmentRepository : IAsyncRepository<Equipment>
    {
        Task<IEnumerable<Equipment>> GetByCategoryId(int CategoryId);
    }
}
