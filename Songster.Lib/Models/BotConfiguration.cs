using Newtonsoft.Json;

namespace Songster.Lib.Models;

public class BotConfiguration {

    [JsonProperty(nameof(Token))]
    public string Token { get; set; } = string.Empty;

    [JsonProperty(nameof(GuildId))]
    public ulong GuildId { get; set; } = 0;

    public BotConfiguration() {
        // Empty constructor
    }
}