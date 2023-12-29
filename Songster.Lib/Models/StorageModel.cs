using Newtonsoft.Json;

namespace Songster.Lib.Models;

public class StorageModel {

    [JsonProperty(nameof(Queue))]
    public List<Song> Queue { get; set; } = new();

    [JsonProperty(nameof(Leaderboard))]
    public Dictionary<ulong, int> Leaderboard { get; set; } = new();

    [JsonProperty(nameof(CurrentUserId))]
    public ulong CurrentUserId { get; set; } = 0;

    [JsonProperty(nameof(Guesses))]
    public Dictionary<ulong, bool> Guesses { get; set; } = new();

    public StorageModel() {

    }

}