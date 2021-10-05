using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechTalks.FixerIo.Client.Recommended
{
    public interface IFixerClient
    {
        Uri BaseAddress { get; }
        string BuildQuery(params KeyValuePair<string, string>[] requestParams);
        Task<IFixerResponse> GetAsync(string path, string request);
    }
}
