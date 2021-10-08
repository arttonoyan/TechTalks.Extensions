using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using System.Collections.Generic;

namespace TechTalks.Hangfire.Recommended
{
    public class BatchBackgroundJobClient : IBatchBackgroundJobClient
    {
        private int _batchSize;
        private int _currentState;
        private readonly IBackgroundJobClient _jobClient;
        private readonly List<string> _jobs;

        public BatchBackgroundJobClient(IBackgroundJobClient jobClient, BachOptions bachOptions)
        {
            _currentState = 0;
            _batchSize = bachOptions.Size;
            _jobClient = jobClient;
            _jobs = new List<string>(_batchSize);
        }

        public int BatchSize => _batchSize;

        public bool ChangeState(string jobId, IState state, string expectedState)
            => _jobClient.ChangeState(jobId, state, expectedState);

        public string Create(Job job, IState state)
        {
            if (state.Name == EnqueuedState.StateName)
            {
                string jobId;
                if (_jobs.Count < _batchSize)
                {
                    jobId = _jobClient.Create(job, state);
                    _jobs.Add(jobId);
                }
                else
                {
                    if (_currentState == _batchSize)
                        _currentState = 0;

                    var parentId = _jobs[_currentState];

                    state = new AwaitingState(parentId, state ?? new EnqueuedState(), JobContinuationOptions.OnlyOnSucceededState);
                    jobId = _jobClient.Create(job, state);
                    _jobs[_currentState] = jobId;
                }

                _currentState++;
                return jobId;
            }
            else
            {
                return _jobClient.Create(job, state);
            }
        }

        public IBackgroundJobClient WithBatchSize(int size)
        {
            _batchSize = size;
            _jobs.Capacity = _batchSize;
            return this;
        }
    }
}
