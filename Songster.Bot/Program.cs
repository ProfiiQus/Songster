using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Songster.Lib;
using Songster.Lib.Models;
using Songster.Lib.Services;

namespace Songster.Bot;

public class Program {

    public static void Main(string[] args) {
        var provider = BuildServices();

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
}