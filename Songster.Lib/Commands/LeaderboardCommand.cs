using Discord;
using Discord.WebSocket;
using Songster.Lib.Services;

namespace Songster.Lib.Commands;

/// <summary>
/// Command that shows the leaderboard.
/// </summary>
public class LeaderboardCommand : ICommand
{
    /// <summary>
    /// Executes the command's logic.
    /// </summary>
    public Task Execute(StorageService storage, SocketSlashCommand command)
    {
        // Instantiate embed builder for response.
        var embedBuilder = new EmbedBuilder()
            .WithTitle("Leaderboard")
            .WithColor(Color.DarkBlue);

        // Now, Let's respond with the embed.
        return command.RespondAsync(embed: embedBuilder.Build());
    }
}