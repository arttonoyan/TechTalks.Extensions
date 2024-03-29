﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TechTalks.FixerIo.Client.Recommended;

namespace TechTalks.FixerIo.Client.ConsoleTestApp
{
    //https://fixer.io/
    class Program : Sample
    {
        public override async Task RunAsync()
        {
            var client = _provider.GetRequiredService<IFixerClient>();
            var res = await client.GetLatestAsync(Symbols.AFN, Symbols.AMD);

            Console.ReadLine();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            const string factoryName = "FixerTest";
            services.AddFixerClient(factoryName, options => options.Configure(cfg =>
            {
                cfg.BaseUrl = "http://data.fixer.io/api/";
                cfg.AccessKey = "5bb9e34a850d88ee925a582135d75262";
            }));
        }

        public static Task<int> Main(string[] args) =>
            RunProgramAsConsoleAsync<Program>(args);
    }
}
