using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Core.Interfaces
{
    public interface IComplaintDetails : IAsyncRepository<ComplaintDetails>
    {
        Task<ComplaintDetails> CreateComplaintAsync(ComplaintDetails ComplainerRequest);
        Task<IEnumerable<ComplaintDetails>> GetComplainerAsync();
        Task<ComplaintDetails> GetComplainerByIdAsync(int ID);

        Task<ComplaintDetails> AssignEscalationAsync(int categoryId, Complaint complaint);
        Task<ComplaintDetails> GetComplaintDetailByID(int ID);
        Task<List<ComplaintDetails>> GetComplaintDetailByComplaintId(int complaintId);

        Task<string> GetNextTicketNo();


    }
}
