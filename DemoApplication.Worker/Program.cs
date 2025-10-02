using DemoApplication.Worker;
using DemoApplication.Worker.Jobs;
using DemoApplication.Worker.Metrics;
using Quartz;
using Quartz.Impl.Matchers;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSingleton<JobMetrics>();
builder.Services.AddHostedService<Worker>();

// This call is required for the DemoApp.Jobs meter to start being collected and exposed
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics => metrics.AddMeter("DemoApp.Jobs"));

builder.Services.Configure<QuartzOptions>(options =>
{
    options.AddJob<DemoJob>(job => job.StoreDurably().WithIdentity(DemoJob.Key));
    options.AddTrigger(
        trigger => trigger.ForJob(DemoJob.Key)
            .WithSimpleSchedule(
                schedule => schedule.RepeatForever().WithIntervalInSeconds(2)));
    options.AddTrigger(trigger => trigger.ForJob(DemoJob.Key)
        .UsingJobData("message", "runs only thrice")
        .WithSimpleSchedule(schedule => schedule.WithRepeatCount(2).WithIntervalInSeconds(4))
    );
    // options.Scheduling.IgnoreDuplicates = true; // default: false
    // options.Scheduling.OverWriteExistingData = true; // default: true
});

builder.Services.AddQuartz(q =>
{
    q.AddJobListener<MetricsJobListener>(GroupMatcher<JobKey>.AnyGroup());

    q.UseSimpleTypeLoader();
    q.UsePersistentStore(options =>
    {
        options.PerformSchemaValidation = true;
        options.UseSqlServer(sqlServerOptions => sqlServerOptions.ConnectionStringName = "myDB");
        options.UseSystemTextJsonSerializer();
    });
    
});
builder.Services.AddQuartzHostedService(options =>
{
    options.AwaitApplicationStarted = true;
    options.WaitForJobsToComplete = true;
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.Run();