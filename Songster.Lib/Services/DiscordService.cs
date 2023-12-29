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
        _client = new DiscordSocketClient(new DiscordSocketConfig {
            AlwaysDownloadUsers = true,
            GatewayIntents = GatewayIntents.GuildMembers
        });
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

        await _client.LoginAsync(TokenType.Bot, _configuration.Token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    /// <summary>
    /// Handles the client ready event.
    /// </summary>
    /// <remarks>
    /// Should be used to register command handlers.
    /// </remarks>
    /// <param name="readyFunc">Function that gets run when the client is ready</param>
    /// <returns></returns>
    public void SetClientReadyHandler(Func<Task> readyFunc) {
        _client.Ready += readyFunc;
    }

    /// <summary>
    /// Sets the client activity.
    /// </summary>
    /// <param name="activity">Activity to set</param>
    public async Task SetClientActivity(IActivity activity) {
        await _client.SetActivityAsync(activity);
    }

    /// <summary>
    /// Registers the guild commands.
    /// </summary>
    /// <param name="guild">The guild to register the commands to</param>
    public async Task RegisterGuildCommands(ulong guildId) {
        var guild = GetGuild(guildId);
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
    /// Gets the guild by the given id.
    /// </summary>
    /// <param name="guildId">Guild Id to get</param>
    /// <returns>SocketGuild object, null if the guild was not found</returns>
    private SocketGuild? GetGuild(ulong guildId) {
        foreach(SocketGuild guild in _client.Guilds) {
            if(guild.Id == guildId) {
                return guild;
            }
        }
        return null;
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

    public IUser GetUser(ulong userId) {
        return _client.Rest.GetUserAsync(userId).Result;
    }
}