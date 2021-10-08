using Microsoft.Extensions.DependencyInjection;
using System;

namespace TechTalks.Hangfire.Standard
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHangfire(this IServiceCollection services) =>
            services
                .ConfigureHangfire()
                .AddScoped<IBackgroundJobClientService, BackgroundJobClientService>();
    }
}
