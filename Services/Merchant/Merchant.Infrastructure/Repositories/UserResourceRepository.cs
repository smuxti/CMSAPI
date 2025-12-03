using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Infrastructure.Repositories
{
    public class UserResourceRepository : AsyncRepository<UserResource> , IUserResourceRepository
    {
        private readonly IConfiguration configuration;

        public UserResourceRepository(MerchantContext context, ILogger<UserResourceRepository> logger, IConfiguration configuration) : base(context, logger)
        {
            this.configuration = configuration;
        }

        public async Task<IEnumerable<UserResource>> GetUserResourcesByMerchId(Guid id)
        {
            var list =await _dbContext.UserResources.Where(x => x.MerchantId == id && x.isDeleted==false).ToListAsync();
            return list;
        }

        public async Task<UserResource> GetResourceByMerchIdAndFileType(Guid merchid, string filetype)
        {
            var res = await _dbContext.UserResources.Where(x => x.MerchantId== merchid && x.FileType==filetype && x.isDeleted==false).FirstOrDefaultAsync();
            return res;
        }
    }
}
