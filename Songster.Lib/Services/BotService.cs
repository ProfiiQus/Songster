using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Songster.Lib.Helpers;
using Songster.Lib.Jobs;
using Songster.Lib.Models;

namespace Songster.Lib.Services;

public class BotService {
    /// <summary>
    /// Bot configuration model.
    /// </summary>
    private BotConfiguration _configuration;

    /// <summary>
    /// Discord service.
    /// </summary>
    private DiscordService _discordService;

    /// <summary>
    /// Slash command service.
    /// </summary>
    private SlashCommandService _slashCommandService;

    /// <summary>
    /// Daily song job.
    /// </summary>
    private DailySongJob _dailySongJob;

    public BotService(IOptions<BotConfiguration> configuration, DiscordService discordService, SlashCommandService slashCommandService, DailySongJob dailySongJob) {
        _configuration = configuration.Value;
        _discordService = discordService;
        _slashCommandService = slashCommandService;
        _dailySongJob = dailySongJob;
    }

    public async Task Start() {
        // Register the daily song scheduler job.
        await SchedulerHelper.ScheduleDailySongJob(_dailySongJob);

        // Register the bot's event handlers.
        _discordService.SetClientReadyHandler(async () => {
            // Set the bot's activity.
            await _discordService.SetClientActivity(new Game("some absolute bangers."));

            // Register commands and command handler.
            _discordService.RegisterSlashCommandHandler(_slashCommandService.SlashCommandHandler);
            await _discordService.RegisterGuildCommands(_configuration.GuildId);
        });

        // Start the bot
        await _discordService.Start();
    }
}