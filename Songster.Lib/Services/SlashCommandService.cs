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

    /// <summary>
    /// TODO: Add docs
    /// </summary>
    /// <param name="configuration"></param>
    public SlashCommandService(IOptions<BotConfiguration> configuration, StorageService storageService) {
        _configuration = configuration.Value;
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
        await commandLibrary[command.Data.Name].Execute(_storageService, command);
    }

    /// <summary>
    /// Registers the guild commands.
    /// </summary>
    /// <param name="guild">The guild to register the commands to</param>
    public async Task RegisterGuildCommands(SocketGuild guild) {
        try
        {
            var command = new SlashCommandBuilder()
                .WithName("about")
                .WithDescription("About the Songster bot");

            await guild.CreateApplicationCommandAsync(command.Build());

            command = new SlashCommandBuilder()
                .WithName("queue")
                .WithDescription("Queue a new banger to daily playlist")
                .AddOption("link", ApplicationCommandOptionType.String, "Link to YouTube song video", isRequired: true);

            await guild.CreateApplicationCommandAsync(command.Build());

            command = new SlashCommandBuilder()
                .WithName("guess")
                .WithDescription("Guess who queued today's banger")
                .AddOption("user", ApplicationCommandOptionType.User, "Who do you think queued today's banger?", isRequired: true);

            await guild.CreateApplicationCommandAsync(command.Build());

            command = new SlashCommandBuilder()
                .WithName("leaderboard")
                .WithDescription("Display's the Songster leaderboard");

            await guild.CreateApplicationCommandAsync(command.Build());
        }
        catch(HttpException exception)
        {
            // Try to handle errors if any occur
            var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }

}