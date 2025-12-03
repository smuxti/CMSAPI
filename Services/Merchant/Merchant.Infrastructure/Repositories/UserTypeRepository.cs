using Authentication.Core.Interfaces;
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
    public class UserTypeRepository : AsyncRepository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(MerchantContext context, ILogger<UserTypeRepository> logger, IConfiguration configuration) : base(context, logger)
        {

        }

        public async Task<UserType> GetByUserTypeCode(int id)
        {
            var a = await _dbContext.UserTypes.Where(x => x.TypeCode == id).FirstOrDefaultAsync();
            return  a;
        }
    }
}
