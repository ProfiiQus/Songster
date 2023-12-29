using Discord.WebSocket;
using Songster.Lib.Services;

namespace Songster.Lib.Commands;

public class GuessCommand : ICommand
{
    public async Task Execute(StorageService storage, SocketSlashCommand command)
    {
        // Parse the guild user from input.
        var guildUser = (SocketGuildUser)command.Data.Options.First().Value;

        // Check if the guild user isn't the same user as the command executor.
        if(guildUser.Id == command.User.Id) {
            await command.RespondAsync("You can't guess yourself, silly.", ephemeral: true);
            return;
        }

        // If the user is the same one who queued the song, respond with an error.
        if(storage.CurrentUserId == guildUser.Id) {
            await command.RespondAsync("You can't guess the same user who queued the song.", ephemeral: true);
            return;
        }

        // If the user has already guessed today, respond with an error.
        if(storage.HasGuessedToday.ContainsKey(command.User.Id)) {
            await command.RespondAsync("You have already guessed today.", ephemeral: true);
            return;
        }

        await command.RespondAsync($"You executed {command.Data.Name}, user {guildUser.Nickname}");
    }
}