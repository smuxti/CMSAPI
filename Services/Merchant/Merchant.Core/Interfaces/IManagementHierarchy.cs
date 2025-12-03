using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface  IManagementHierarchy : IAsyncRepository<ManagementHierarchy>
    {
        Task<IEnumerable<ManagementHierarchy>> GetManagementHierarchyAsyn();
        Task<ManagementHierarchy> AddManagementHierarchyAsync(ManagementHierarchy managementHierarchy);

        Task<ManagementHierarchy> GetManagementHierarchyByID(int Id);
        Task<IEnumerable<ManagementHierarchy>> GetManagementHierarchyByParentId(int Id);
    }
}
