using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Data
{
    internal class AuthenticationContextFactory : IDesignTimeDbContextFactory<AuthenticationContext>
    {
        public AuthenticationContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<AuthenticationContext>();
            optionBuilder.UseSqlServer("Data Source=AuthenticationDB");
            return new AuthenticationContext(optionBuilder.Options);
        }
    }
}
