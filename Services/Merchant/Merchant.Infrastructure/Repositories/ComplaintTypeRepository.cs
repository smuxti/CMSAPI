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
    public class ComplaintTypeRepository: AsyncRepository<ComplaintType>, IComplaintType
    {
        private readonly MerchantContext _merchantContext;
        private readonly ILogger<ComplaintTypeRepository> _logger;

        public ComplaintTypeRepository(MerchantContext merchantContext, ILogger<ComplaintTypeRepository> logger)
            : base(merchantContext, logger)
        {
            _merchantContext = merchantContext;
            _logger = logger;
        }


        public Task<IEnumerable<ComplaintType>> GetComplaintTypeAsyn()
        {
            throw new NotImplementedException();
        }

        public async Task<ComplaintType> GetType(int ID)
        {
            var ManagementHierarchy = await _merchantContext.ComplaintTypes.Where(x => x.ID == ID).FirstOrDefaultAsync();


            return ManagementHierarchy;
        }
    }
}
