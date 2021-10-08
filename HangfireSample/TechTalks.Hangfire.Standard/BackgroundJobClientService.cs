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
        }

        public void BachEnqueue(Expression<Action> methodCall)
        {
            if (_jobs.Count < _bachSize)
            {
                _jobs.Add(_jobClient.Enqueue(methodCall));
            }
            else
            {
                if (_currentState == _bachSize)
                    _currentState = 0;

                string jobId = _jobs[_currentState];
                _jobs[_currentState] = _jobClient.ContinueJobWith(jobId, methodCall);
            }

            _currentState++;
        }
    }
}
