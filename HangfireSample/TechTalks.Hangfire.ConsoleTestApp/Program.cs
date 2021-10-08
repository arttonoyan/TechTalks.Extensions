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

            //using var scope = _provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            //var provider = scope.ServiceProvider;

            for (int i = 0; i < 30; i++)
            {
                var jobClientSrvice = _provider.GetRequiredService<IBackgroundJobClientService>();
                jobClientSrvice.BachEnqueue(() => TestMethod(i));
            }
            
            return Task.CompletedTask;
        }

        public void TestMethod(int i)
        {
            Console.WriteLine($"Barev {i}");
            Task.Delay(1000).GetAwaiter().GetResult();
        }

        protected override void ConfigureServices(IServiceCollection services) =>
            services.AddHangfireServices();

        public static Task<int> Main(string[] args) =>
            RunProgramAsHostAsync<Program>(args);
    }
}
