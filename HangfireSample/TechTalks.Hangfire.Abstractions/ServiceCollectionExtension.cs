using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TechTalks.Hangfire.Standard
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureHangfire(this IServiceCollection services) =>
            services
                .AddHangfire(cfg => cfg.UseInMemoryStorage())
                .AddHangfireServer();

        public static IServiceCollection ConfigureHangfire(this IServiceCollection services, Action<HangfireOptions> optionsConfigurator)
        {
            var options = new HangfireOptions();
            optionsConfigurator.Invoke(options);

            services.AddHangfire(cfg =>
            {
                cfg.UseRecommendedSerializerSettings();
                cfg.UseSqlServerStorage(options.ConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });
            });

            services.AddHangfireServer();
            return services;
        }
    }
}
