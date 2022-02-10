using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Contrib.HttpClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TechTalks.FixerIo.Client.Recommended.Tests
{
    public class FixerIoClientTests
    {
        private readonly Mock<HttpMessageHandler> _httpHandler = new();
        private IServiceProvider Configure()
        {
            var services = new ServiceCollection();

            services.AddFixerClient("FixerTest", options => options.Configure(cfg =>
            {
                cfg.BaseUrl = "http://data.fixer.io/api/";
                cfg.AccessKey = "testKey";
            }));

            services.AddHttpClient<IFixerClient, FixerClient>()
                .ConfigurePrimaryHttpMessageHandler(() => _httpHandler.Object);

            return services.BuildServiceProvider();
        }

        [Theory]
        [JsonFileData("FixerIoLatestContent.json")]
        public async Task GetAsyncTest(string data)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(data)
            };

            //_httpHandler
            //    .SetupAnyRequest()
            //    .ReturnsAsync(response);

            _httpHandler
                .SetupRequest(message
                    => message.RequestUri.AbsoluteUri == "http://data.fixer.io/api/myMethod?access_key=testKey&A1")
                .ReturnsAsync(new HttpResponseMessage()
                {
                    Content = new StringContent(data)
                });

            var provider = Configure();

            var client = provider.GetService<IFixerClient>();
            var res = await client.GetAsync("myMethod", "A1");
        }
    }
}
