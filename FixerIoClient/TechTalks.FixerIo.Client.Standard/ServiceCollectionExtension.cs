﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace TechTalks.FixerIo.Client.Standard
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddFixerClient(this IServiceCollection services, string name, Action<OptionsBuilder<FixerOptions>> optionsBuilder) =>
            services
                .AddScoped<IFixerClient, FixerClient>()
                .ConfigureFixerClient(name, optionsBuilder);
    }
}
