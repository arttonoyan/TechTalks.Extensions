using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechTalks.FixerIo.Client.Standard
{
    public interface IFixerClient
    {
        public Uri BaseAddress { get; }
        Task<IFixerResponse> GetLatestAsync();
        Task<IFixerResponse> GetLatestAsync(IEnumerable<string> symbols);
        Task<IFixerResponse> GetLatestAsync(IEnumerable<Symbols> symbols);
        Task<IFixerResponse> GetLatestAsync(params Symbols[] symbols);

        Task<IFixerResponse> GetHistoricalAsync(DateTime date);
        Task<IFixerResponse> GetHistoricalAsync(DateTime date, IEnumerable<string> symbols);
        Task<IFixerResponse> GetHistoricalAsync(DateTime date, params Symbols[] symbols);
    }
}
