using Microsoft.Extensions.DependencyInjection;

namespace TechTalks.Hangfire.Standard
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHangfireServices(this IServiceCollection services) =>
            services
                .ConfigureHangfire()
                .AddSingleton(new BachOptions { Size = 5 })
                .AddScoped<IBackgroundJobClientService, BackgroundJobClientService>();
    }
}
