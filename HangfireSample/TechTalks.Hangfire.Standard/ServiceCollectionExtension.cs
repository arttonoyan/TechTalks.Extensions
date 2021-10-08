using Microsoft.Extensions.DependencyInjection;
using System;

namespace TechTalks.Hangfire.Standard
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFixerClient(this IServiceCollection services) =>
            services;
                //.AddScoped<IFixerClient, FixerClient>()
                //.ConfigureFixerClient(name, optionsBuilder);
    }
}
