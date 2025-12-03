using Authentication.Core.Interfaces;
using Authentication.Infrastructure.Data;
using Authentication.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Authentication.Infrastructure.Extensions
{
    public static class InfrastructureServices
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var conString = configuration.GetConnectionString("DefaultConnection");
            var server = Environment.GetEnvironmentVariable("MSSQL_HOST");
            var dbName = Environment.GetEnvironmentVariable("MSSQL_DB_NAME");
            var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
            conString = string.Format(conString, server, dbName, password);
            services.AddDbContext<AuthenticationContext>(option => option.UseSqlServer(conString));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserResourceRepository, UserResourceRepository>();
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            //services.AddSingleton<IConnectionMultiplexer>(options =>ConnectionMultiplexer.Connect(("127.0.0.1:6379")));


            return services;
        }
    }
}
