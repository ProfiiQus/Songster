using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Songster.Lib.Jobs;

public class DailySongJobFactory : IJobFactory {

    private readonly IServiceProvider _serviceProvider;

    public DailySongJobFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return _serviceProvider.GetRequiredService<DailySongJob>();
    }

    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;
        disposable?.Dispose();
    }
}