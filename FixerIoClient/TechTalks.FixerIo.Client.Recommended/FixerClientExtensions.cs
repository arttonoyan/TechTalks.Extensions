using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechTalks.FixerIo.Client.Recommended
{
    public static class FixerClientExtensions
    {
        public static Task<IFixerResponse> GetLatestAsync(this IFixerClient fixerClient)
        {
            string query = fixerClient.BuildQuery();
            return fixerClient.GetAsync("latest", query);
        }

        public static Task<IFixerResponse> GetLatestAsync(this IFixerClient fixerClient, IEnumerable<string> symbols)
        {
            var symbolsValue = string.Join(',', symbols);
            var query = fixerClient.BuildQuery(KeyValuePair.Create("symbols", symbolsValue));
            return fixerClient.GetAsync("latest", query);
        }

        public static Task<IFixerResponse> GetLatestAsync(this IFixerClient fixerClient, IEnumerable<Symbols> symbols)
        {
            return fixerClient.GetLatestAsync(symbols.Select(p => p.ToString()));
        }

        public static Task<IFixerResponse> GetLatestAsync(this IFixerClient fixerClient, params Symbols[] symbols)
        {
            return fixerClient.GetLatestAsync(symbols.Select(p => p.ToString()));
        }

        public static Task<IFixerResponse> GetHistoricalAsync(this IFixerClient fixerClient, DateTime date)
        {
            string query = fixerClient.BuildQuery();
            return fixerClient.GetAsync(date.ToString("yyyy-MM-dd"), query);
        }

        public static Task<IFixerResponse> GetHistoricalAsync(this IFixerClient fixerClient, DateTime date, IEnumerable<string> symbols)
        {
            var symbolsValue = string.Join(',', symbols);
            string query = fixerClient.BuildQuery(KeyValuePair.Create("symbols", symbolsValue));
            return fixerClient.GetAsync(date.ToString("yyyy-MM-dd"), query);
        }

        public static Task<IFixerResponse> GetHistoricalAsync(this IFixerClient fixerClient, DateTime date, params Symbols[] symbols)
        {
            return fixerClient.GetHistoricalAsync(date, symbols.Select(p => p.ToString()));
        }
    }
}
