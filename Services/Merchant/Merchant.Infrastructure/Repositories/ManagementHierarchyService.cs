using Azure;
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
    public class ManagementHierarchyService : AsyncRepository<ManagementHierarchy>, IManagementHierarchy
    {
        private readonly MerchantContext _merchantContext;
        private readonly ILogger<ManagementHierarchyService> _logger;

        public ManagementHierarchyService(MerchantContext merchantContext, ILogger<ManagementHierarchyService> logger)
            : base(merchantContext, logger)
        {
            _merchantContext = merchantContext;
            _logger = logger;
        }

        public Task<ManagementHierarchy> AddManagementHierarchyAsync(ManagementHierarchy managementHierarchy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ManagementHierarchy>> GetManagementHierarchyAsyn()
        {
            throw new NotImplementedException();
        }

        public async Task<ManagementHierarchy> GetManagementHierarchyByID(int Id)
        {

            var ManagementHierarchy = await _merchantContext.ManagementHierarchies.Where(x => x.ID == Id && x.isDeleted == false).FirstOrDefaultAsync();


            return ManagementHierarchy;


        }
        public async Task<IEnumerable<ManagementHierarchy>> GetManagementHierarchyByParentId(int Id)
        {

            var ManagementHierarchy = await _merchantContext.ManagementHierarchies.Where(x => x.ParentID == Id && x.isDeleted == false && x.ManagementType == 4).ToListAsync();


            return ManagementHierarchy;


        }
    }
}
