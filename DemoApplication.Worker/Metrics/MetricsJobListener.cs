using Quartz;

namespace DemoApplication.Worker.Metrics;

public class MetricsJobListener(JobMetrics jobMetrics) : IJobListener
{
    public string Name => nameof(MetricsJobListener);
    
    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        jobMetrics.JobStarted(context.JobDetail.JobType);
        return Task.CompletedTask;
    }

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.CompletedTask;
    }

    public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (jobException is null)
        {
            jobMetrics.JobFinished(context.JobDetail.JobType);
        }
        else
        {
            jobMetrics.JobFailed(context.JobDetail.JobType);
        }
        return Task.CompletedTask;
    }

}