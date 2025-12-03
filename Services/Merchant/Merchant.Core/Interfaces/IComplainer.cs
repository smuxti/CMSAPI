using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IComplainer :IAsyncRepository<Complainer>
    {
        Task<Complainer> GetComplainerByEmail(string mobile);
        Task<Complainer> GetCmplainerByID(int ID);
    }
}
