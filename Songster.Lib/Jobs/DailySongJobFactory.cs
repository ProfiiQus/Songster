using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Songster.Lib.Jobs;

/// <summary>
/// Job factory for the daily song job.
/// </summary>
public class DailySongJobFactory : IJobFactory {

    /// <summary>
    /// Daily song job.
    /// </summary>
    private readonly DailySongJob _dailySongJob;

    /// <summary>
    /// Creates a new instance of the daily song job factory.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <remarks>
    /// Constructor values are passed by Dependency Injection and not instantiated manually.
    /// </remarks>
    public DailySongJobFactory(DailySongJob dailySongJob) {
        _dailySongJob = dailySongJob;
    }

    /// <summary>
    /// Creates a new instance of the daily song job.
    /// </summary>
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _dailySongJob;
    }

    /// <summary>
    /// Returns the job, disposes of it.
    /// </summary>
    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;
        disposable?.Dispose();
    }
}