using System.Windows.Input;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Songster.Lib.Commands;
using Songster.Lib.Models;

namespace Songster.Lib.Services;

/// <summary>
/// Services that manages interaction with slash commands and registers them.
/// </summary>
public class SlashCommandService {

    /// <summary>
    /// Dictionary of available command names and their implementations.
    /// </summary>
    private Dictionary<string, Commands.ICommand> commandLibrary = new Dictionary<string, Commands.ICommand> {
        { "about", new AboutCommand() },
        { "queue", new QueueCommand() },
        { "guess", new GuessCommand() },
        { "leaderboard", new LeaderboardCommand() }
    };

    /// <summary>
    /// Bot configuration model.
    /// </summary>
    private BotConfiguration _configuration;

    private StorageService _storageService;

    private DiscordService _discordService;

    /// <summary>
    /// TODO: Add docs
    /// </summary>
    /// <param name="configuration"></param>
    public SlashCommandService(IOptions<BotConfiguration> configuration, DiscordService discordService, StorageService storageService) {
        _configuration = configuration.Value;
        _discordService = discordService;
        _storageService = storageService;
    }

    /// <summary>
    /// Function that handles the slash commands.
    /// </summary>
    /// <param name="command">Command name</param>
    public async Task SlashCommandHandler(SocketSlashCommand command) {
        // Check if the command's functionality is defined.
        if (!commandLibrary.ContainsKey(command.Data.Name)) {
            await command.RespondAsync($"The command entered has undefined workflow. Contact an administrator.");
            return;
        }
        
        // If defined, run it.
        await commandLibrary[command.Data.Name].Execute(_storageService, _discordService, command);
    }

}