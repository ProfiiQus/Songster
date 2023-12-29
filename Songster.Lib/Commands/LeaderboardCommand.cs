using System.Text;
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
            .WithColor(Color.Gold);

        // Load top 5 players from leaderboard
        var top5 = storage.Leaderboard
            .OrderByDescending(x => x.Value)
            .Take(5);

        // If leaderboard is empty, append to embed that there are no players. Return.
        if(top5.Count() == 0) {
            embedBuilder.WithDescription("There are no players on the leaderboard.");
            return command.RespondAsync(embed: embedBuilder.Build());
        }

        var firstUser = command.Channel.GetUserAsync(top5.First().Key).Result;

        embedBuilder.WithDescription($"🏆 **{firstUser.GlobalName}** is currently the best guesser with {top5.First().Value} points.");
        embedBuilder.WithThumbnailUrl(firstUser.GetAvatarUrl());

        var topStringBuilder = new StringBuilder();

        // Add top 5 players to the embed.
        int index = 1;
        foreach(var player in top5) {
            var user = command.Channel.GetUserAsync(player.Key).Result;
            topStringBuilder.AppendLine($"{GetRankMedal(index)} **{user.GlobalName}** - {player.Value} points");
            index++;
        }

        embedBuilder.AddField("Top 3 players", topStringBuilder.ToString());

        // Now, Let's respond with the embed.
        return command.RespondAsync(embed: embedBuilder.Build());
    }

    private string GetRankMedal(int index) {
        switch(index) {
            case 1:
                return "🥇";
            case 2:
                return "🥈";
            case 3:
                return "🥉";
            default:
                return "🏅";
        }
    }
}