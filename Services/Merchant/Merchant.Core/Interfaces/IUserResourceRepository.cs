using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IUserResourceRepository : IAsyncRepository<UserResource>
    {
        Task<IEnumerable<UserResource>> GetUserResourcesByMerchId(Guid id);
        Task<UserResource> GetResourceByMerchIdAndFileType(Guid merchid,string filetype);
    }
}
