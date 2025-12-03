using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IComplaintCategory:IAsyncRepository<ComplaintCategory>
    {
        Task<IEnumerable<ComplaintCategory>> GetComplaintCategoryAsyn();
        Task<ComplaintCategory> AddComplaintCategoryAsync(ComplaintCategory complaintCategory);

        Task<ComplaintCategory> GetComplaintCategoryByID(int ID);
    }
}
