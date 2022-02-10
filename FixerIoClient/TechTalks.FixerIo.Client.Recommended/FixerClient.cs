using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TechTalks.FixerIo.Client.Recommended
{
    public class FixerClient : IFixerClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessKey;

        public FixerClient(HttpClient httpClient, IOptionsSnapshot<FixerOptions> optionsFactory)
        {
            _httpClient = httpClient;

            var options = optionsFactory.Get("FixerTest");
            _httpClient.BaseAddress = new Uri(options.BaseUrl);
            _accessKey = options.AccessKey;
        }

        public Uri BaseAddress => _httpClient.BaseAddress;

        public Task<IFixerResponse> GetAsync(string path, string query = null)
        {
            try
            {
                return InnerGetAsync(path, query);
            }
            catch (Exception ex)
            {
                //TODO [log] [Artem Tonoyan] [12/09/2021]: Add log.
                return Task.FromResult<IFixerResponse>(new FixerResponse
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
        }

        private async Task<IFixerResponse> InnerGetAsync(string path, string query)
        {
            var request = HttpQueryBuilder.BuildRequest(path, KeyValuePair
                .Create("access_key", _accessKey), query);

            using var response = await _httpClient.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return new FixerResponse(() => JsonSerializer.Deserialize<Currency>(content, JsonHelper.DefaultOptions))
                {
                    HttpStatusCode = response.StatusCode,
                    Content = content
                };
            }

            return new FixerResponse { HttpStatusCode = response.StatusCode };
        }
    }
}
