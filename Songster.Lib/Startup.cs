using Microsoft.Extensions.DependencyInjection;
using Songster.Lib.Jobs;
using Songster.Lib.Services;

namespace Songster.Lib;

public static class Startup {

    /// <summary>
    /// Adds the libraries services to the service collection.
    /// </summary>
    /// <param name="collection">Collection to add the services to</param>
    /// <returns>Collection with added services</returns>
    public static IServiceCollection RegisterServices(IServiceCollection collection) {
        collection
            .AddSingleton<DiscordService>()
            .AddSingleton<SlashCommandService>()
            .AddSingleton<BotService>()
            .AddSingleton<StorageService>()
            .AddSingleton<DailySongJob>();
        return collection;
    }

}