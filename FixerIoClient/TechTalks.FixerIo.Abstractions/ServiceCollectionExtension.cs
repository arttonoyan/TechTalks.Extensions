using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace TechTalks.FixerIo
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureFixerClient(this IServiceCollection services, string name, Action<OptionsBuilder<FixerOptions>> optionsBuilder)
        {
            optionsBuilder.Invoke(services.AddOptions<FixerOptions>(name));
            services
                .AddHttpClient(name, (p, client) =>
                {
                    var options = p.GetService<IOptionsSnapshot<FixerOptions>>().Get(name);
                    client.BaseAddress = new Uri(options.BaseUrl);
                });

            return services;
        }
    }
}
