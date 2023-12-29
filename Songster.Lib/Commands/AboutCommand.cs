using Discord;
using Discord.WebSocket;
using Songster.Lib.Services;

namespace Songster.Lib.Commands;

/// <summary>
/// Class representation of a an about command.
/// </summary>
/// <remarks>
/// This command is primarily used to display information about the bot to the user.
/// </remarks>
public class AboutCommand : ICommand
{
    /// <summary>
    /// Performs the execution of the command.
    /// </summary>
    public async Task Execute(StorageService storage, DiscordService discordService, SocketSlashCommand command)
    {
        // Instantiate embed builder for response.
        var embedBuilder = new EmbedBuilder()
            .WithTitle("About the Songster")
            .WithDescription("👋 Hello there. Ich bin Songster, the Hipster song bot. I'm here to **recommend some new bangers** you can listen to throughout the day.")
            .AddField("Daily banger", "Queue **up to 5 daily bangers** using the `/queue` command. Every day at **10:00 AM**, a new banger drops. You can then guess who queued today's banger with the `/guess` command. You have a **single guess** each day.")
            .AddField("Best guessers", "You can check the best guessers using the `/leaderboard` command.")
            .AddField("Source code", "You can find my source code or suggest new features on [GitHub](https://github.com/ProfiiQus/Songster).")
            .WithColor(Color.DarkBlue);

        // Now, Let's respond with the embed.
        await command.RespondAsync(embed: embedBuilder.Build());
    }
}