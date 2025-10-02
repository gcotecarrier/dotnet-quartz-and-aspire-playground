using System.Diagnostics.Metrics;

namespace DemoApplication.Worker.Metrics;

public class JobMetrics
{
        private readonly Counter<int> _jobsStarted;
        private readonly Counter<int> _jobsFinished;
        private readonly Counter<int> _jobsFailed;

        public JobMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create("DemoApp.Jobs");
            _jobsStarted = meter.CreateCounter<int>("demoapp.jobs.started");
            _jobsFinished = meter.CreateCounter<int>("demoapp.jobs.finished");
            _jobsFailed = meter.CreateCounter<int>("demoapp.jobs.failed");

        }

        public void JobStarted(Type jobType)
        {
            _jobsStarted.Add(1, new KeyValuePair<string, object?>("job.type",jobType.Name));
        }
        
        public void JobFinished(Type jobType)
        {
            _jobsFinished.Add(1, new KeyValuePair<string, object?>("job.type",jobType.Name));
        }
        
        public void JobFailed(Type jobType)
        {
            _jobsFailed.Add(1, new KeyValuePair<string, object?>("job.type",jobType.Name));
        }
}