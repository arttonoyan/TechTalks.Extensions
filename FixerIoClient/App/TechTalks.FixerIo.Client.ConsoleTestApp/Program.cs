﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TechTalks.FixerIo.Client.Standard;

namespace TechTalks.FixerIo.Client.ConsoleTestApp
{
    //https://fixer.io/
    class Program
    {
        protected static IServiceProvider _provider;

        static async Task Main(string[] args)
        {
            Configure();

            var client = _provider.GetRequiredService<IFixerClient>();
            var res = await client.GetLatestAsync(Symbols.AFN, Symbols.AMD);

            Console.ReadLine();
        }

        private static void Configure()
        {
            var services = new ServiceCollection();

            const string factoryName = "FixerTest";
            services.AddFixerClient(factoryName, options => options.Configure(cfg =>
            {
                cfg.BaseUrl = "http://data.fixer.io/api/";
                cfg.AccessKey = "some-key";
            }));

            _provider = services.BuildServiceProvider();
        }
    }
}
