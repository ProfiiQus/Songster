using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Songster.Lib.Jobs;

namespace Songster.Lib.Helpers;

/// <summary>
/// Helper class for job scheduling.
/// </summary>
public static class SchedulerHelper {

    /// <summary>
    /// Schedules the daily song job every day at 10AM.
    /// </summary>
    public static async Task ScheduleDailySongJob(ServiceProvider serviceProvider) {
        Console.WriteLine("Scheduled!");
        // Grab the Scheduler instance from the Factory
        var factory = new StdSchedulerFactory();
        var scheduler = await factory.GetScheduler();

        // Start the scheduler
        await scheduler.Start();

        Console.WriteLine("Scheduler: " + scheduler.IsStarted);

        // Define the job and tie it to our DailySongJob class
        IJobDetail job = JobBuilder.Create<DailySongJob>()
            .WithIdentity("DailySongJob", "Songster")
            .Build();

        // Trigger the job to run every day at 10AM
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("DailySongTrigger", "Songster")
            .WithSchedule(
                CronScheduleBuilder.DailyAtHourAndMinute(1, 43)
                .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Europe/Prague")))
            .ForJob(job)
            .StartNow()
            .Build();

        scheduler.JobFactory = new DailySongJobFactory(serviceProvider);

        // Schedule the job using the job and trigger
        await scheduler.ScheduleJob(job, trigger);
    }
}