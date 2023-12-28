using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Logging;
using Songster.Lib;
using Songster.Lib.Helpers;
using Songster.Lib.Models;
using Songster.Lib.Services;

namespace Songster.Bot;

public class Program {

    public static void Main(string[] args) {
        LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

        // Build service provider.
        var provider = BuildServices();

        // Schedule daily song job.
        _ = SchedulerHelper.ScheduleDailySongJob(provider);

        // Start the Discord bot serving.
        var bot = provider.GetRequiredService<BotService>();
        bot.Start().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Builds dependency injection service provider.
    /// </summary>
    /// <returns>Service provider with system services</returns>
    public static ServiceProvider BuildServices()
    {
        // Build configuration
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = configurationBuilder.Build();

        // Build service collection
        var serviceCollection = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .Configure<BotConfiguration>(configuration.GetSection(nameof(BotConfiguration)));

        serviceCollection = Startup.RegisterServices(serviceCollection);

        return serviceCollection.BuildServiceProvider();
    }

    private class ConsoleLogProvider : ILogProvider
{
    public Logger GetLogger(string name)
    {
        return (level, func, exception, parameters) =>
        {
            if (level >= LogLevel.Info && func != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
            }
            return true;
        };
    }

    public IDisposable OpenNestedContext(string message)
    {
        throw new NotImplementedException();
    }

    public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
    {
        throw new NotImplementedException();
    }
}
}