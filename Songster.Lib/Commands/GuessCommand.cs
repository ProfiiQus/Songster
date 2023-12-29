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

        // If the user has already guessed today, respond with an error.
        if(storage.Guesses.ContainsKey(command.User.Id)) {
            await command.RespondAsync("You have already guessed today.", ephemeral: true);
            return;
        }

        // Print that the user has placed a guess but don't disclose whether it's correct or not.
        await command.Channel.SendMessageAsync($"<@{command.User.Id}> has placed a guess.");

        // Otherwise check if the user has guessed correctly.
        // Print display but don't display the result.
        if(storage.CurrentUserId == guildUser.Id) {
            // Register that the player has guessed correctly.
            storage.Guesses.Add(command.User.Id, true);
            storage.Save();

            // Respond with a success message.
            await command.RespondAsync($"You guessed correctly!", ephemeral: true);
        } else {
            // Register that the player has guessed wrong.
            storage.Guesses.Add(command.User.Id, false);
            storage.Save();

            // Response with a failure message.
            await command.RespondAsync($"You guessed incorrectly!", ephemeral: true);
        }
    }
}