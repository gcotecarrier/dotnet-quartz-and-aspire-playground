using Quartz;

namespace DemoApplication.Worker.Jobs;

public class DemoJob(ILogger<DemoJob> logger): IJob
{
    public static JobKey Key = new("Demo", "Job");
    
    public Task Execute(IJobExecutionContext context)
    {
        context.MergedJobDataMap.TryGetString("message", out var message);
        message ??= "default";
        
        logger.LogInformation("Hello from Quartz job {job} and message {message}", context.FireInstanceId, message);
        return Task.CompletedTask;
    }
}