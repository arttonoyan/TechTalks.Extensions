using Hangfire;

namespace TechTalks.Hangfire.Recommended
{
    public interface IBatchBackgroundJobClient : IBackgroundJobClient
    {
        int BatchSize { get; }
        IBackgroundJobClient WithBatchSize(int size);
    }
}
