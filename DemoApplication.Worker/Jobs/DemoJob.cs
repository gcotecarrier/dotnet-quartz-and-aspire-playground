using Quartz;

namespace DemoApplication.Worker.Jobs;

public class DemoJob(ILogger<DemoJob> logger) : IJob
{
    public static JobKey Key = new("Demo", "Job");
    private static Random random = new();

    public async Task Execute(IJobExecutionContext context)
    {
        context.MergedJobDataMap.TryGetString("message", out var message);
        message ??= "default";

        await Task.Delay(random.Next(3000, 4000));

        logger.LogInformation("Hello from Quartz job {job} and message {message}", context.FireInstanceId, message);
    }
}