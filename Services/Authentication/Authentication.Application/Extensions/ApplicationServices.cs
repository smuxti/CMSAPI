using Authentication.Application.Behaviours;
using EmailManager.MailService;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetSection("Redis")["ConnectionString"];
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg=> cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(UnhandledExceptionBehaviour<,>));
            services.AddSingleton<IConnectionMultiplexer>(options =>
            ConnectionMultiplexer.Connect((connection)));
            services.AddHttpContextAccessor();
            services.AddSingleton<Mail>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var smtpSettings = configuration.GetSection("SmtpSettings");

                return new Mail(
                    smtpSettings["Server"],
                    int.Parse(smtpSettings["Port"]),
                    smtpSettings["User"],
                    smtpSettings["Pass"]);
            });
            return services;
        }
    }
}
