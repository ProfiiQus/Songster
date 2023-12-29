using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Songster.Lib.Models;

namespace Songster.Lib.Services;

/// <summary>
/// Service for interaction with Discord API.
/// </summary>
/// <remarks>
/// TODO: Add exception handling when Guild is not returned.
/// </remarks>
public class DiscordService {

    /// <summary>
    /// Discord bot configuration model.
    /// </summary>
    private BotConfiguration _configuration { get; set;}

    /// <summary>
    /// Discord socket client.
    /// </summary>
    private DiscordSocketClient _client;

    /// <summary>
    /// Discord service primary constructor.
    /// </summary>
    /// <param name="configuration">Bot configuration object</param>
    public DiscordService(IOptions<BotConfiguration> configuration) {
        _configuration = configuration.Value;
        _client = new DiscordSocketClient();
    }

    /// <summary>
    /// Creates a new application command.
    /// </summary>
    /// <param name="properties">Command to register</param>
    /// <param name="guildId">Guild to register to command to</param>!
    public void CreateApplicationCommandAsync(ApplicationCommandProperties properties, ulong guildId) {
        _client.Rest.CreateGuildCommand(properties, guildId);
    }

    /// <summary>
    /// Registers the slash command handler.
    /// </summary>
    /// <param name="handler">Handler function to register</param>
    public void RegisterSlashCommandHandler(Func<SocketSlashCommand, Task> handler) {
        _client.SlashCommandExecuted += handler;
    }

    /// <summary>
    /// Registers the bot's event handlers and starts the bot.
    /// </summary>
    public async Task Start() {
        _client.Log += Log;
        _client.Ready += Client_Ready;

        await _client.LoginAsync(TokenType.Bot, _configuration.Token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private async Task Client_Ready() {
        var guild = _client.GetGuild(_configuration.GuildId);

        await _client.SetActivityAsync(new Game("some absolute bangers"));
    }

    /// <summary>
    /// Registers the guild commands.
    /// </summary>
    /// <param name="guild">The guild to register the commands to</param>
    public async Task RegisterGuildCommands(ulong guildId) {
        var guild = _client.GetGuild(guildId);
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

    /// <summary>
    /// Logs the given message to the console.
    /// </summary>
    /// <param name="message">Message to log</param>
    private Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sends the given message to the given channel.
    /// </summary>
    /// <param name="channelId">Channel Id to send the message to</param>
    /// <param name="embed">Embed to send</param>
    public void SendEmbed(ulong channelId, Embed embed) {
        var channel = _client.GetChannel(channelId) as IMessageChannel;
        channel?.SendMessageAsync(embed: embed);
    }

    public SocketUser? GetUserById(ulong userId) {
        return _client.GetUser(userId);
    }
}