using Newtonsoft.Json;

namespace Songster.Lib.Models;

public class BotConfiguration {

    [JsonProperty(nameof(Token))]
    public string Token { get; set; } = string.Empty;

    [JsonProperty(nameof(GuildId))]
    public ulong GuildId { get; set; } = 0;

    /// <summary>
    /// The channel ID of the channel where the bot will post the daily song.
    /// </summary>
    [JsonProperty(nameof(DailySongChannelId))]
    public ulong DailySongChannelId { get; set; } = 0;

    /// <summary>
    /// The current song (the user who queued it).
    /// </summary>
    [JsonProperty(nameof(CurrentQueuerId))]
    public ulong CurrentQueuerId { get; set; } = 0;

    public BotConfiguration() {
        // Empty constructor
    }
}