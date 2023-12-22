using Discord;
using Discord.WebSocket;
using Songster.Lib.Models;
using Songster.Lib.Services;

namespace Songster.Lib.Commands;

public class QueueCommand : ICommand
{

    public async Task Execute(StorageService storage, SocketSlashCommand command)
    {
        var song = new Song {
            UserId = command.User.Id,
            Link = command.Data.Options.First().Value.ToString()!
        };

        var embedBuilder = new EmbedBuilder();

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