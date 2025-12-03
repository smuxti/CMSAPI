using Merchants.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Merchants.Core.Common;

namespace Merchants.Core.Interfaces
{
    public interface IComplaint : IAsyncRepository<Complaint>
    {
        Task<Complaint> CreateComplaintAsync(Complaint complaintRequest);
        Task<IList<ComplainView>> GetComplaintsAsync();
        Task<Complaint> GetComplaintByIdAsync(int ID);
        //Task<string> GetNextTicketNo();
        Task<IList<ComplaintHistoryView>> GetComlaintHistory(int ID);
        Task<IList<ComplainView>> GetComplaintByManagmentIdAsync(int ManagementID);
        Task<List<Complaint>> GetYearlyComplaints(string year);
        Task<List<Complaint>> GetWeeklyComplaints(string year, string? month, string? week);
        Task<List<ComplaintDetails>> GetWeeklyComplaintDetails(string year, string? month, string? week);
        Task<List<Complaint>> GetMonthlyComplaints(string year, string month);
        Task<int> GetNoOfComplainToday();
        Task<int> GetNoOfPendingComplainToday();
        Task<int> GetTotalClosedComplain();
        Task<Int16> GetAveragResponseTime();

        Task<IList<ComplaintHistoryView>> GetComlaintMerchantHistory(int ID);

    }
}