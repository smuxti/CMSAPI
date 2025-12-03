using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Merchants.Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Infrastructure.Repositories
{
    public class ComplaintDetailsRepository : AsyncRepository<ComplaintDetails>, IComplaintDetails
    {
        private readonly MerchantContext _merchantContext;
        private readonly ILogger<ComplaintDetailsRepository> _logger;

        public ComplaintDetailsRepository(MerchantContext merchantContext, ILogger<ComplaintDetailsRepository> logger)
            : base(merchantContext, logger)
        {
            _merchantContext = merchantContext;
            _logger = logger;
        }

        public Task<ComplaintDetails> AssignEscalationAsync(int categoryId, Complaint complaint)
        {
            throw new NotImplementedException();
        }

        public async Task<ComplaintDetails> CreateComplaintAsync(ComplaintDetails ComplainerRequest)
        {
            ComplainerRequest.TickentNo = await GetNextTicketNo();
            var _escObj = await _merchantContext.AddAsync(ComplainerRequest);
            await _merchantContext.SaveChangesAsync();

            return _escObj.Entity;
        }

        public Task<IEnumerable<ComplaintDetails>> GetComplainerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ComplaintDetails> GetComplainerByIdAsync(int ID)
        {
            throw new NotImplementedException();
        }
        public async Task<ComplaintDetails> GetComplaintDetailByID(int ID)
        {
            var dtl = await _merchantContext.ComplaintDetails.Where(w => w.ID == ID && w.isDeleted == false).FirstOrDefaultAsync();
            return dtl;
        }
        public async Task<List<ComplaintDetails>> GetComplaintDetailByComplaintId(int complaintId)
        {
            var dtl = await _merchantContext.ComplaintDetails.Where(w => w.ComplaintID == complaintId && w.isDeleted == false).ToListAsync();
            return dtl;
        }

        public async Task<string> GetNextTicketNo()
        {
            string temp = string.Empty;
            using (var transaction = _merchantContext.Database.BeginTransaction())
            {
                try
                {
                    temp = _merchantContext.ComplaintDetails.Max(x => x.TickentNo);
                    //string temp = _merchantContext.ComplaintDetails.FromSqlRaw("SELECT TOP 1 TickentNo FROM tblComplaintDetails WITH (UPDLOCK) ORDER BY TickentNo DESC",param).FirstOrDefaultAsync();
                    if (temp == null) temp = DateTime.Now.Year.ToString().Substring(2, 2) + "000001";
                    else temp = (int.Parse(temp) + 1).ToString();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            //string temp = await _merchantContext.ComplaintDetails.MaxAsync(x => x.TickentNo);
            ////string temp = _merchantContext.ComplaintDetails.FromSqlRaw("SELECT TOP 1 TickentNo FROM tblComplaintDetails WITH (UPDLOCK) ORDER BY TickentNo DESC",param).FirstOrDefaultAsync();
            //if (temp == null) temp = DateTime.Now.Year.ToString().Substring(2, 2) + "000001";
            //else temp = (int.Parse(temp) + 1).ToString();
            return temp;
        }
    }
}
