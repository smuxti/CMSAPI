using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IUserTypeRepository : IAsyncRepository<UserType>
    {
        Task<UserType> GetByUserTypeCode(int id);
    }
}
