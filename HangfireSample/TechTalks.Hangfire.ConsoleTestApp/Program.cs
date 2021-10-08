using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TechTalks.Hangfire.Standard;

namespace TechTalks.Hangfire.ConsoleTestApp
{
    class Program : Sample
    {
        public override Task RunAsync()
        {
            ////just for test
            //var jobClient = _provider.GetRequiredService<IBackgroundJobClient>();
            //jobClient.Enqueue(() => Console.WriteLine("Barev"));
            //jobClient.Enqueue<IMyService>(s => s.TestMethod(1));

            for (int i = 0; i < 30; i++)
            {
                var jobClientSrvice = _provider.GetRequiredService<IBackgroundJobClientService>();
                jobClientSrvice
                    .WithBatchSize(2)
                    .BatchEnqueue(() => TestMethod(i));
            }

            return Task.CompletedTask;
        }

        public void TestMethod(int i)
        {
            Console.WriteLine($"Barev {i}");
            Task.Delay(3000).GetAwaiter().GetResult();
        }

        protected override void ConfigureServices(IServiceCollection services) =>
            services
                .AddHangfireServices()
                .AddScoped<IMyService, MyService>();

        public static Task<int> Main(string[] args) =>
            RunProgramAsHostAsync<Program>(args);
    }
}
