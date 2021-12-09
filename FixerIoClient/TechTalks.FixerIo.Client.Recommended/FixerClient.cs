using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TechTalks.FixerIo.Client.Recommended
{
    public class FixerClient : IFixerClient
    {
        private readonly HttpClient _httpClient;
        private readonly FixerOptions _fixerOptions;

        public FixerClient(IHttpClientFactory httpClientFactory, IOptionsSnapshot<FixerOptions> fixerOptions)
        {
            const string name = "FixerTest";
            _fixerOptions = fixerOptions.Get(name);
            _httpClient = httpClientFactory.CreateClient(name);
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
            var accessKeyQuey = $"access_key={_fixerOptions.AccessKey}";
            if (!string.IsNullOrWhiteSpace(query))
                accessKeyQuey = $"{accessKeyQuey}&{query}";

            var request = $"{path}?{query}";
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
