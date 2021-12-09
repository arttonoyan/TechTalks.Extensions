using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechTalks.FixerIo.Client.Recommended
{
    public interface IFixerClient
    {
        Uri BaseAddress { get; }
        Task<IFixerResponse> GetAsync(string path, string query = null);
    }
}
