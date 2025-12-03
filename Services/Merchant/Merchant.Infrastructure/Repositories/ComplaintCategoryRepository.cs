using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Infrastructure.Repositories
{
    public class ComplaintCategoryRepository : AsyncRepository<ComplaintCategory>, IComplaintCategory
    {
        private readonly MerchantContext _merchantContext;
        private readonly ILogger<ComplaintCategoryRepository> _logger;

        public ComplaintCategoryRepository(MerchantContext merchantContext, ILogger<ComplaintCategoryRepository> logger)
            : base(merchantContext, logger)
        {
            _merchantContext = merchantContext;
            _logger = logger;
        }

        //public ComplaintCategoryRepository(MerchantContext merchantContext, ILogger<ComplaintCategoryRepository> logger) : base(merchantContext, logger)
        //{

        //}

        public async Task<ComplaintCategory> GetComplaintCategoryByID(int ID)
        {
            var category = await _merchantContext.ComplaintCategories
                                                 .Where(c => c.ID == ID)
                                                 .FirstOrDefaultAsync();

            return category;
        }


        public async Task<ComplaintCategory> AddComplaintCategoryAsync(ComplaintCategory complaintCategory)
        {
            //return await _dbContext.AddAsync(complaintCategory);
            throw new NotImplementedException();
        }

        //public async Task<IEnumerable<MerchantTransactionType>> GetByMerchantID(Guid MerchantID)
        //{
        //    return await _dbContext.merchantTransactionTypes.Where(o => o.MerchantID.Equals(MerchantID)).ToListAsync();
        //}

        public Task<IEnumerable<ComplaintCategory>> GetComplaintCategoryAsyn()
        {
            throw new NotImplementedException();
        }

        //public Task<ComplaintCategory> GetComplaintCategoryByID(int ID)
        //{
        //    throw new NotImplementedException();
        //}
    }



    //internal class MerchantTransactionTypeRepository : AsyncRepository<MerchantTransactionType>, IMerchantTransactionTypeRepository
    //{
       
    //}
}
