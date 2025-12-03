using Authentication.Core.Interfaces;
using Merchants.Core.Entities;
using Merchants.Core.Interfaces;
using Merchants.Infrastructure.Data;
//using Merchants.Infrastructure.OneLink;
using Merchants.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Merchants.Infrastructure.Extentions
{
    public static class InfraServices
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var conString = configuration.GetConnectionString("DefaultConnection");
            var server = Environment.GetEnvironmentVariable("MSSQL_HOST");
            var dbName = Environment.GetEnvironmentVariable("MSSQL_DB_NAME");
            var password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
            conString = string.Format(conString, server, dbName, password);
            serviceCollection.AddDbContext<MerchantContext>(option => option.UseSqlServer(conString));
            serviceCollection.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            serviceCollection.AddSingleton<IRedisCacheService, RedisCacheService>();
            serviceCollection.AddScoped<IComplaintCategory, ComplaintCategoryRepository>();
            serviceCollection.AddScoped<IChannel, ChannelRepository>();
            serviceCollection.AddScoped<IComplaint, ComplaintRepository>();
            serviceCollection.AddScoped<IComplaintType, ComplaintTypeRepository>();
            serviceCollection.AddScoped<IEscalation, EscalationService>();
            serviceCollection.AddScoped<IManagementHierarchy, ManagementHierarchyService>();
            serviceCollection.AddScoped<IComplaintDetails, ComplaintDetailsRepository>();
            serviceCollection.AddScoped<IComplainer, ComplainerRepository>();
            serviceCollection.AddScoped<IMerchant, MerchantService>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUserResourceRepository, UserResourceRepository>();
            serviceCollection.AddScoped<IZones, ZonesRepository>();
            serviceCollection.AddScoped<INotificationRepo, NotificationRepo>();
            serviceCollection.AddScoped<IUserTypeRepository, UserTypeRepository>();
            serviceCollection.AddScoped<IEquipmentRepository, EquipmentRepository>();

            return serviceCollection;
        }
    }
}
