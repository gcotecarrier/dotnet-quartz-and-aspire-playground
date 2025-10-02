using System.Diagnostics.Metrics;

namespace DemoApplication.Worker.Metrics;

public class JobMetrics
{
        private readonly Counter<int> _jobsStarted;
        private readonly Counter<int> _jobsFinished;
        private readonly Counter<int> _jobsFailed;
        private readonly UpDownCounter<int> _jobsActive;

        public JobMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create("DemoApp.Jobs");
            _jobsStarted = meter.CreateCounter<int>("demoapp.jobs.started");
            _jobsFinished = meter.CreateCounter<int>("demoapp.jobs.finished");
            _jobsFailed = meter.CreateCounter<int>("demoapp.jobs.failed");
            _jobsActive = meter.CreateUpDownCounter<int>("demoapp.jobs.active");
        }

        public void JobStarted(Type jobType)
        {
            var context = new KeyValuePair<string, object?>("job.type",jobType.Name);
            _jobsStarted.Add(1, context);
            _jobsActive.Add(1, context);
        }
        
        public void JobFinished(Type jobType)
        {
            var context = new KeyValuePair<string, object?>("job.type",jobType.Name);
            _jobsFinished.Add(1, context);
            _jobsActive.Add(-1, context);
        }
        
        public void JobFailed(Type jobType)
        {
            _jobsFailed.Add(1, new KeyValuePair<string, object?>("job.type",jobType.Name));
        }
}