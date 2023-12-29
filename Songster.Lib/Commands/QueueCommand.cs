using Discord;
using Discord.WebSocket;
using Songster.Lib.Models;
using Songster.Lib.Services;

namespace Songster.Lib.Commands;

/// <summary>
/// Class representation of a a queue command.
/// </summary>
/// <remarks>
/// The purpose of this command is to allow users to queue new songs.
/// </remarks>
public class QueueCommand : ICommand
{

    /// <summary>
    /// Performs the execution of the command.
    /// </summary>
    public async Task Execute(StorageService storage, DiscordService discordService, SocketSlashCommand command)
    {
        // Build a new song object from the provided arguments.
        var song = new Song {
            UserId = command.User.Id,
            Link = command.Data.Options.First().Value.ToString()!
        };

        // Instantiate embed builder for response.
        var embedBuilder = new EmbedBuilder();

        // Attempt to queue the song.
        // Respond with a success or failure message.
        if(storage.Queue(song)) {
            embedBuilder = new EmbedBuilder()
                .WithTitle("Banger successfully queued!")
                .WithColor(Color.Green)
                .WithDescription("✅ I have queued your suggestion.");
        } else {
            embedBuilder = new EmbedBuilder()
                .WithTitle("Already at max queue!")
                .WithColor(Color.Red)
                .WithDescription("❌ You already have 5 songs queued bozo.");
        }

        // Now, Let's respond with the embed.
        await command.RespondAsync(embed: embedBuilder.Build());
    }
}