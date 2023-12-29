using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Songster.Lib.Models;

namespace Songster.Lib.Services;

public class BotService {
    /// <summary>
    /// Bot configuration model.
    /// </summary>
    private BotConfiguration _configuration;

    /// <summary>
    /// Discord client.
    /// </summary>
    private DiscordSocketClient _client;

    /// <summary>
    /// Slash command service.
    /// </summary>
    private SlashCommandService _slashCommandService;

    public BotService(IOptions<BotConfiguration> configuration, SlashCommandService slashCommandService) {
        _configuration = configuration.Value;
        _client = new DiscordSocketClient();
        _slashCommandService = slashCommandService;
    }

    /// <summary>
    /// Starts the bot.
    /// </summary>
    public async Task Start() {
        _client.Log += Log;
        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += _slashCommandService.SlashCommandHandler;

        await _client.LoginAsync(TokenType.Bot, _configuration.Token);
        await _client.StartAsync();

        await Task.Delay(-1);
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

    private async Task Client_Ready() {
        var guild = _client.GetGuild(_configuration.GuildId);
        await _slashCommandService.RegisterGuildCommands(guild);

        await _client.SetActivityAsync(new Game("some absolute bangers"));
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
}