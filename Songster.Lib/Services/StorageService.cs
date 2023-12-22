using System.Text.Json;
using Songster.Lib.Models;

namespace Songster.Lib.Services;

public class StorageService {

    private StorageModel _storage;

    public StorageService() {
        if(!File.Exists("storage.json")) {
            _storage = new StorageModel();
            Save();
        }

        var json = File.ReadAllText("storage.json");
        _storage = JsonSerializer.Deserialize<StorageModel>(json)!;
    }

    public bool Queue(Song song) {
        // Check if the same user id doesn't already have 5 songs queued
        var count = _storage.Queue.Count(x => x.UserId == song.UserId);
        if(count >= 5) {
            return false;
        }

        _storage.Queue.Enqueue(song);
        Save();
        return true;
    }

    public void Save() {
        var json = JsonSerializer.Serialize(_storage);
        File.WriteAllText("storage.json", json);
    }
}