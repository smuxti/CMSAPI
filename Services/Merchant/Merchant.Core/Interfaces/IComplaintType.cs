using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IComplaintType:IAsyncRepository<ComplaintType> 
    {
        Task<IEnumerable<ComplaintType>> GetComplaintTypeAsyn();
        //Task<ComplaintType> AddComplaintTypeAsync(ComplaintTypeDto complaintType);

        Task<ComplaintType> GetType(int ID);
    }
}
