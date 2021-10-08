using System;
using System.Linq.Expressions;

namespace TechTalks.Hangfire.Standard
{
    public interface IBackgroundJobClientService
    {
        void Enqueue(Expression<Action> methodCall);
    }
}
