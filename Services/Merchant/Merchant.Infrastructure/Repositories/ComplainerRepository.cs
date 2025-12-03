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
    public class ComplainerRepository : AsyncRepository<Complainer>, IComplainer
    {

        public ComplainerRepository(MerchantContext merchantContext, ILogger<ComplainerRepository> logger) : base(merchantContext, logger)
        {

        }

        public async Task<Complainer> GetComplainerByEmail(string mobile)
        {
            var complainer = await _dbContext.Complainers.Where(x => x.Mobile == mobile && x.isDeleted==false).FirstOrDefaultAsync();
            return complainer;
        }

        public async Task<Complainer> GetCmplainerByID(int ID)
        {
            var complainer = await _dbContext.Complainers.Where(x=> x.ID == ID).FirstOrDefaultAsync();
            return complainer;
        }

    }
}
