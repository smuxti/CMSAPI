using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Merchants.API.Extensions
{
    public static class DBExtension
    {
        public static IHost MigrateDatabse<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope()) { 
            
                var service = scope.ServiceProvider;
                var logger = service.GetRequiredService<ILogger<TContext>>();
                var context = service.GetService<TContext>();
                try
                {
                    logger.LogInformation($"Database migration started: {typeof(TContext).Name}");
                    CallSeeder(seeder, context, service);
                    logger.LogInformation($"Migration Completed: {typeof(TContext).Name}");
                }
                catch (SqlException ex)
                {

                    logger.LogError($"An error occured during db migration: {typeof(TContext).Name}");
                }
            }
            return host;
        }

        private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider service) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, service);
        }
    }
}
