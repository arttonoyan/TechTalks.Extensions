using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        public Task<IFixerResponse> GetAsync(string path, string request)
        {
            try
            {
                return InnerGetAsync(path, request);
            }
            catch (Exception ex)
            {
                IFixerResponse response = new FixerResponse();
                return Task.FromResult(response);
            }
        }

        private async Task<IFixerResponse> InnerGetAsync(string path, string query)
        {
            var request = $"{path}?{query}";
            using var response = await _httpClient.GetAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return new FixerResponse
                {
                    HttpStatusCode = response.StatusCode,
                    Content = content,
                    Currency = JsonSerializer.Deserialize<Currency>(content, JsonHelper.DefaultOptions)
                };
            }

            return new FixerResponse
            {
                HttpStatusCode = response.StatusCode
            };
        }

        public string BuildQuery(params KeyValuePair<string, string>[] requestParams)
        {
            var request = $"access_key={_fixerOptions.AccessKey}";
            if (requestParams != null && requestParams.Length > 0)
            {
                var builder = new StringBuilder();
                foreach (var (key, value) in requestParams)
                {
                    builder.Append(key).Append('=').Append(value).Append('&');
                }

                request = $"{request}&{builder}";
            }

            return request.TrimEnd('&');
        }
    }
}
