using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Merchants.Infrastructure.Repositories
{
    class TerminalRepository : AsyncRepository<Terminal>, ITerminalRepository
    {
        public TerminalRepository(MerchantContext merchantContext, ILogger<TerminalRepository> logger) : base(merchantContext, logger)
        {
        }
        public async Task<List<Terminal>> GetTerminalByMerchantID(Guid merchantID)
        {
            return await _dbContext.Terminals.Where(o => o.MerchantID == merchantID).ToListAsync();
        }

        public async Task<Terminal> GetTerminalBySerial(string serial)
        {
            return await _dbContext.Terminals.Where(o => o.SerialNumber.Equals(serial)).FirstOrDefaultAsync();
        }
        public async Task<Terminal> GetTerminalByCode(string code)
        {
            return await _dbContext.Terminals.Where(o => o.SerialNumber.Equals(code)).FirstOrDefaultAsync();
        }
        public async Task<Terminal> GetTerminalByCodeAndMerchantId(string code, Guid merchantId)
        {
            return await _dbContext.Terminals.Where(o => o.SerialNumber.Equals(code) && o.MerchantID == merchantId).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Tenant>> GetAllTenants()
        {

            return await _dbContext.Tenants.ToListAsync();
        }
    }
}
