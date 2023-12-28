using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Songster.Lib.Jobs;

/// <summary>
/// Job factory for the daily song job.
/// </summary>
public class DailySongJobFactory : IJobFactory {

    /// <summary>
    /// The service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new instance of the daily song job factory.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <remarks>
    /// Constructor values are passed by Dependency Injection and not instantiated manually.
    /// </remarks>
    public DailySongJobFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Creates a new instance of the daily song job.
    /// </summary>
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _serviceProvider.GetRequiredService<DailySongJob>();
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