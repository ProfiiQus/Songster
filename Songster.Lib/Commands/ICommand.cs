using Discord.WebSocket;
using Songster.Lib.Services;

namespace Songster.Lib.Commands;

/// <summary>
/// Interface for creation of new commands.
/// </summary>
public interface ICommand {

    /// <summary>
    /// Executes the command's logic.
    /// </summary>
    /// <param name="storageService">Storage service</param>
    /// <param name="discordService">Discord service</param>
    /// <param name="command">Command that was executed</param>
    public Task Execute(StorageService storageService, DiscordService discordService, SocketSlashCommand command);
}