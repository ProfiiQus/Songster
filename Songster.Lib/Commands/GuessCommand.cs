using Discord.WebSocket;
using Songster.Lib.Services;

namespace Songster.Lib.Commands;

public class GuessCommand : ICommand
{
    public async Task Execute(StorageService storage, SocketSlashCommand command)
    {
        var guildUser = (SocketGuildUser)command.Data.Options.First().Value;
        await command.RespondAsync($"You executed {command.Data.Name}, user {guildUser.Nickname}");
    }
}