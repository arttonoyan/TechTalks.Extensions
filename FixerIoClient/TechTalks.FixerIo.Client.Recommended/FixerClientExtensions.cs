using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechTalks.FixerIo.Client.Recommended
{
    public static class FixerClientExtensions
    {
        public static Task<IFixerResponse> GetLatestAsync(this IFixerClient fixerClient) =>
            fixerClient.GetAsync("latest");

        public static Task<IFixerResponse> GetLatestAsync(this IFixerClient fixerClient, IEnumerable<string> symbols)
        {
            var symbolsValue = string.Join(',', symbols);
            var query = HttpQueryBuilder.Build(KeyValuePair.Create("symbols", symbolsValue));
            return fixerClient.GetAsync("latest", query);
        }

        public static Task<IFixerResponse> GetLatestAsync(this IFixerClient fixerClient, IEnumerable<Symbols> symbols) =>
            GetLatestAsync(fixerClient, symbols.Select(p => p.ToString()));

        public static Task<IFixerResponse> GetLatestAsync(this IFixerClient fixerClient, params Symbols[] symbols) =>
            GetLatestAsync(fixerClient, symbols.Select(p => p.ToString()));

        public static Task<IFixerResponse> GetLatestAsync(this IFixerClient fixerClient, params string[] symbols) =>
            GetLatestAsync(fixerClient, symbols.AsEnumerable());

        public static Task<IFixerResponse> GetHistoricalAsync(this IFixerClient fixerClient, DateTime date) =>
            fixerClient.GetAsync(date.ToString("yyyy-MM-dd"), HttpQueryBuilder.Build());

        public static Task<IFixerResponse> GetHistoricalAsync(this IFixerClient fixerClient, DateTime date, IEnumerable<string> symbols)
        {
            var symbolsValue = string.Join(',', symbols);
            string query = HttpQueryBuilder.Build(KeyValuePair.Create("symbols", symbolsValue));
            return fixerClient.GetAsync(date.ToString("yyyy-MM-dd"), query);
        }

        public static Task<IFixerResponse> GetHistoricalAsync(this IFixerClient fixerClient, DateTime date, params Symbols[] symbols) =>
            GetHistoricalAsync(fixerClient, date, symbols.Select(p => p.ToString()));
    }
}
