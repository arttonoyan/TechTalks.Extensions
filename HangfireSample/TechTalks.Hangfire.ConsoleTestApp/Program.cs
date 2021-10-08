using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TechTalks.Hangfire.ConsoleTestApp
{
    class Program : Sample
    {
        public override Task RunAsync()
        {
            //For test
            var jobClient = _provider.GetRequiredService<IBackgroundJobClient>();
            jobClient.Enqueue(() => Console.WriteLine("Barev"));



            return Task.CompletedTask;
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(cfg => cfg.UseInMemoryStorage());
            services.AddHangfireServer();
        }

        public static Task<int> Main(string[] args) =>
            RunProgramAsHostAsync<Program>(args);
    }
}
