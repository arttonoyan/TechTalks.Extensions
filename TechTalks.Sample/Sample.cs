using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TechTalks
{
    public abstract class Sample
    {
        protected IServiceProvider _provider;

        public async Task ConfigureServicesAndRunSample(string[] args, Func<Task> run)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _provider = services.BuildServiceProvider();

            await run.Invoke();
        }

        public abstract Task RunAsync();
        protected abstract void ConfigureServices(IServiceCollection services);

        protected virtual void Configuration(IServiceProvider serviceProvider) { }

        public static IHostBuilder CreateHostBuilder<TProgram>(string[] args, TProgram app) where TProgram : Sample =>
            Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureLogging(logging => logging.ClearProviders())
                .ConfigureServices(services => app.ConfigureServices(services));

        public static async Task<int> RunProgramAsHostAsync<TProgram>(string[] args)
            where TProgram : Sample, new()
        {
            try
            {
                var app = new TProgram();
                using var host = CreateHostBuilder(args, app).Build();
                app._provider = host.Services;
                await app.RunAsync();
                await host.RunAsync();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.ToString());
                return 1;
            }
            return 0;
        }

        public static async Task<int> RunProgramAsConsoleAsync<TProgram>(string[] args)
            where TProgram : Sample, new()
        {
            try
            {
                var app = new TProgram();
                await app.ConfigureServicesAndRunSample(args, app.RunAsync);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.ToString());
                return 1;
            }
            return 0;
        }
    }
}
