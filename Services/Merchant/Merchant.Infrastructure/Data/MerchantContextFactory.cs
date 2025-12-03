using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Merchants.Infrastructure.Data
{
    internal class MerchantContextFactory : IDesignTimeDbContextFactory<MerchantContext>
    {
        public MerchantContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<MerchantContext>();
            optionBuilder.UseSqlServer("Data Source=MerchantDB");
            return new MerchantContext(optionBuilder.Options);
        }
    }
}
