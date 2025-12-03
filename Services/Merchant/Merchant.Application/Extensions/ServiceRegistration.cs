using EmailManager.MailService;
using FluentValidation;
using MediatR;
using Merchants.Application.BackgroundJobs;
using Merchants.Application.Behaviours;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using StackExchange.Redis;
using System.Reflection;

namespace Merchants.Application.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetSection("Redis")["ConnectionString"];
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
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

            services.AddQuartz(options =>
            {
                var jobKey = new JobKey("GetAllChannelByQueryJob");

                options.AddJob<GetAllChannelQueryJob>(jobKey)
                       .AddTrigger(trigger => trigger
                           .ForJob(jobKey)
                           .WithSimpleSchedule(schedule =>
                               schedule.WithIntervalInMinutes(1).RepeatForever()));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            return services;
        }
    }
}
