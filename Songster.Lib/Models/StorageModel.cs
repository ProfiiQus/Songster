using Newtonsoft.Json;

namespace Songster.Lib.Models;

public class StorageModel {

    [JsonProperty(nameof(Queue))]
    public Queue<Song> Queue { get; set; } = new Queue<Song>();

    [JsonProperty(nameof(Leaderboard))]
    public Dictionary<ulong, int> Leaderboard { get; set; } = new Dictionary<ulong, int>();

    [JsonProperty(nameof(CurrentUserId))]
    public ulong CurrentUserId { get; set; } = 0;

    [JsonProperty(nameof(HasGuessedToday))]
    public Dictionary<ulong, bool> HasGuessedToday { get; set; } = new Dictionary<ulong, bool>();

    public StorageModel() {

    }

}