using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace TechTalks.FixerIo.Client.Recommended
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFixerClient(this IServiceCollection services, string name, Action<OptionsBuilder<FixerOptions>> optionsBuilder)
        {
            services
                .ConfigureFixerClient(name, optionsBuilder)
                .AddHttpClient<IFixerClient, FixerClient>();
            
            return services;
        }
    }
}
