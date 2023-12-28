using Discord;
using Newtonsoft.Json;

namespace Songster.Lib.Models;

public class Song {

    [JsonProperty(nameof(UserId))]
    public ulong UserId { get; set; } = 0;

    [JsonProperty(nameof(Link))]
    public string Link { get; set; } = string.Empty;

    public Embed BuildEmbed() {
        var embed = new EmbedBuilder()
            .WithTitle("Song of the day")
            .WithDescription($"[Listen to the song]({Link})")
            .WithColor(Color.Blue)
            .Build();

        return embed;
    }
}