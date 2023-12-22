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
    /// <param name="command">Command instance</param>
    public Task Execute(StorageService storage, SocketSlashCommand command);
}