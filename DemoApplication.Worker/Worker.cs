using Quartz;
using DemoApplication.Worker.Jobs;
namespace DemoApplication.Worker;

/// <summary>
/// This class is essentially an example of how to create a standard .NET worker process
/// In our case, we're just using it to create a job trigger that resolves in the future as an example of how to do that
/// The Quartz framework doesn't need this background service to exist at all
/// </summary>
/// <param name="schedulerFactory"></param>
public class Worker(ISchedulerFactory schedulerFactory) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var trigger = TriggerBuilder.Create()
            .ForJob(DemoJob.Key)
            .UsingJobData("message", $"this has been scheduled as a one-off in the past at {DateTime.Now}")
            .StartAt(DateBuilder.FutureDate(20, IntervalUnit.Second))
            .Build();
        
        var scheduler = await schedulerFactory.GetScheduler(stoppingToken);
        await scheduler.ScheduleJob(trigger, stoppingToken);
        
        await scheduler.TriggerJob(DemoJob.Key, new JobDataMap() { ["message"] = "this has been triggered directly as a one-off"}, stoppingToken);
        
    }
}