using Quartz;
using DemoApplication.Worker.Jobs;
namespace DemoApplication.Worker;

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