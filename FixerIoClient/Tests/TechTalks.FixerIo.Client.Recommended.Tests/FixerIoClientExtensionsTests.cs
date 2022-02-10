using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TechTalks.FixerIo.Client.Recommended.Tests
{
    public class FixerIoClientExtensionsTests
    {
        private IServiceProvider Configure()
        {
            var services = new ServiceCollection();

            services.AddFixerClient("FixerTest", options => options.Configure(cfg =>
            {
                cfg.BaseUrl = "http://data.fixer.io/api/";
                cfg.AccessKey = "testKey";
            }));

            services.AddHttpClient<IFixerClient, FixerIoClientMock>();

            return services.BuildServiceProvider();
        }

        [Fact]
        public async Task LatestAsyncTest()
        {
            var provider = Configure();
            var client = provider.GetService<IFixerClient>();
            var res = await client.GetLatestAsync();

            Assert.Equal("latest-", res.Content);
        }

        [Fact]
        public async Task LatestAsyncWithArgTest()
        {
            var provider = Configure();
            var client = provider.GetService<IFixerClient>();
            var res = await client.GetLatestAsync("A1", "A2", "A3");

            Assert.Equal("latest-symbols=A1,A2,A3", res.Content);
        }

        public class FixerIoClientMock : IFixerClient
        {
            public FixerIoClientMock(HttpClient httpClient) { }

            public Uri BaseAddress => null;

            public Task<IFixerResponse> GetAsync(string path, string query)
            {
                return Task.FromResult<IFixerResponse>(new FixerResponse
                {
                    Content = $"{path}-{query}",
                    HttpStatusCode = System.Net.HttpStatusCode.OK
                }); ;
            }
        }
    }
}
