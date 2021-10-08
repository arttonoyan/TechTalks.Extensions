using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TechTalks.Hangfire.Standard
{
    public class BackgroundJobClientService : IBackgroundJobClientService
    {
        private int _bachSize;
        private int _currentState;
        private readonly IBackgroundJobClient _jobClient;
        private readonly List<string> _jobs;

        public BackgroundJobClientService(IBackgroundJobClient jobClient, BachOptions bachOptions)
        {
            _jobClient = jobClient;
            _bachSize = bachOptions.Size;
            _jobs = new List<string>();
        }

        public IBackgroundJobClientService WithBatchSize(int size)
        {
            _bachSize = size;
            return this;
        }

        public string BatchEnqueue(Expression<Action> methodCall)
        {
            string jobId;
            if (_jobs.Count < _bachSize)
            {
                jobId = _jobClient.Enqueue(methodCall);
                _jobs.Add(jobId);
            }
            else
            {
                if (_currentState == _bachSize)
                    _currentState = 0;

                var parentId = _jobs[_currentState];

                jobId = _jobClient.ContinueJobWith(parentId, methodCall);
                _jobs[_currentState] = jobId;
            }

            _currentState++;
            return jobId;
        }
    }
}
