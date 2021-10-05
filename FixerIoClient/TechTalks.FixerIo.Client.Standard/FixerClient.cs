using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TechTalks.FixerIo.Client.Standard
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

        public Task<IFixerResponse> GetLatestAsync()
        {
            string query = BuildQuery();
            return GetAsync("latest", query);
        }

        public Task<IFixerResponse> GetLatestAsync(IEnumerable<string> symbols)
        {
            var symbolsValue = string.Join(',', symbols);
            var query = BuildQuery(KeyValuePair.Create("symbols", symbolsValue));
            return GetAsync("latest", query);
        }

        public Task<IFixerResponse> GetLatestAsync(IEnumerable<Symbols> symbols)
        {
            return GetLatestAsync(symbols.Select(p => p.ToString()));
        }

        public Task<IFixerResponse> GetLatestAsync(params Symbols[] symbols)
        {
            return GetLatestAsync(symbols.Select(p => p.ToString()));
        }

        public Task<IFixerResponse> GetHistoricalAsync(DateTime date)
        {
            string query = BuildQuery();
            return GetAsync(date.ToString("yyyy-MM-dd"), query);
        }

        public Task<IFixerResponse> GetHistoricalAsync(DateTime date, IEnumerable<string> symbols)
        {
            var symbolsValue = string.Join(',', symbols);
            string query = BuildQuery(KeyValuePair.Create("symbols", symbolsValue));
            return GetAsync(date.ToString("yyyy-MM-dd"), query);
        }

        public Task<IFixerResponse> GetHistoricalAsync(DateTime date, params Symbols[] symbols)
        {
            return GetHistoricalAsync(date, symbols.Select(p => p.ToString()));
        }

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
