using System.Text.Json;
using Songster.Lib.Exceptions;
using Songster.Lib.Models;

namespace Songster.Lib.Services;

public class StorageService {


    /// <summary>
    /// Current user Id field wrapper.
    /// Works dynamically with storage to automatically save the value to disk.
    /// </summary>
    public ulong CurrentUserId {
        get {
            return _storage.CurrentUserId;
        }
        set {
            _storage.CurrentUserId = value;
            Save();
        }
    }

    /// <summary>
    /// Current dictionary of guesses.
    /// Works dynamically with storage to automatically save the value to disk.
    /// </summary>
    public Dictionary<ulong, bool> HasGuessedToday {
        get {
            return _storage.HasGuessedToday;
        }
        set {
            _storage.HasGuessedToday = value;
            Save();
        }
    }

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

        _storage.Queue.Add(song);
        Save();
        return true;
    }

    /// <summary>
    /// Pops a random song from the queue.
    /// </summary>
    /// <returns>Randomly popped song</returns>
    /// <exception cref="EmptyQueueException">Thrown when the queue is empty</exception>
    public Song PopRandom() {
        // Check if the queue is not empty.
        // If it is, throw EmptyQueueException.
        if(_storage.Queue.Count == 0) {
            throw new EmptyQueueException();
        }

        // Get a random song from the queue.
        var random = new Random();
        var index = random.Next(0, _storage.Queue.Count);
        var song = _storage.Queue.ElementAt(index);
        
        // Remove the song from the queue and save the storage.
        _storage.Queue.Remove(song);
        Save();
        return song;
    }

    public void Save() {
        var json = JsonSerializer.Serialize(_storage);
        File.WriteAllText("storage.json", json);
    }
}