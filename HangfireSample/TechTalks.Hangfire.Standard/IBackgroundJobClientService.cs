using System;
using System.Linq.Expressions;

namespace TechTalks.Hangfire.Standard
{
    public interface IBackgroundJobClientService
    {
        int BatchSize { get; }
        IBackgroundJobClientService WithBatchSize(int size);
        string BatchEnqueue(Expression<Action> methodCall);
    }
}
