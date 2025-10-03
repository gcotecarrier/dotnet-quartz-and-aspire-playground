using System.Diagnostics.Metrics;
using Microsoft.Extensions.Diagnostics.Metrics;
using TagNameAttribute = Microsoft.Extensions.Diagnostics.Metrics.TagNameAttribute;

using Quartz;

namespace DemoApplication.Worker.Metrics;


public record JobContext
{
    [TagName("job.type")]
    public required string JobType;

    [TagName("job.key")]
    public required string JobKey;

    [TagName("trigger.key")]
    public required string TriggerKey;
}

public partial class JobMetrics
{
    [Counter<int>(typeof(JobContext), Name = "jobs.started")]
    public static partial JobsStarted CreateJobsStarted(Meter meter);

    [Counter<int>(typeof(JobContext), Name = "jobs.finished")]
    public static partial JobsFinished CreateJobsFinished(Meter meter);

    [Counter<int>(typeof(JobContext), Name = "jobs.active")]
    public static partial JobsActive CreateJobsActive(Meter meter);

    private readonly JobsStarted _jobsStarted;
    private readonly JobsFinished _jobsFinished;
    private readonly JobsActive _jobsActive;
    public JobMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("DemoApp.Jobs");
        _jobsStarted = CreateJobsStarted(meter);
        _jobsFinished = CreateJobsFinished(meter);
        _jobsActive = CreateJobsActive(meter);
    }

    public void JobStarted(IJobExecutionContext context)
    {
        JobContext properties = new()
        {
            JobType = context.JobDetail.JobType.Name,
            JobKey = context.JobDetail.Key.ToString(),
            TriggerKey = context.Trigger.Key.ToString(),
        };
        _jobsStarted.Add(1, properties);
        _jobsActive.Add(1, properties);
    }

    public void JobFinished(IJobExecutionContext context)
    {
        JobContext properties = new()
        {
            JobType = context.JobDetail.JobType.Name,
            JobKey = context.JobDetail.Key.ToString(),
            TriggerKey = context.Trigger.Key.ToString(),
        };
        _jobsFinished.Add(1, properties);
        _jobsActive.Add(-1, properties);
    }
}