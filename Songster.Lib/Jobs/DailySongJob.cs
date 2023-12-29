using Discord;
using Microsoft.Extensions.Options;
using Quartz;
using Songster.Lib.Exceptions;
using Songster.Lib.Models;
using Songster.Lib.Services;

namespace Songster.Lib.Jobs;

/// <summary>
/// Class representation of a daily song job.
/// </summary>
/// <remarks>
/// This job is used to display the current song of the day.
/// It randomly selects a song from the queue and displays it in the provided Discord channel.
/// Then removes the given song from the queue and enables song guessing til the next day.
/// </remarks>
public class DailySongJob : IJob
{
    /// <summary>
    /// Discord service.
    /// </summary>
    public DiscordService _discordService { get; set;}

    /// <summary>
    /// Storage service.
    /// </summary>
    public StorageService _storageService { get; set; }

    /// <summary>
    /// Bot configuration model.
    /// </summary>
    public BotConfiguration _configuration { get; set;}

    /// <summary>
    /// Creates a new instance of the daily song job.
    /// </summary>
    /// <remarks>
    /// Constructor values are passed by Dependency Injection and not instantiated manually.
    /// </remarks>
    public DailySongJob(IOptions<BotConfiguration> configuration, DiscordService discordService, StorageService storageService)
    {
        _configuration = configuration.Value;
        _discordService = discordService;
        _storageService = storageService;
    }

    /// <summary>
    /// Performs the job execution.
    /// </summary>
    /// <remarks>
    /// The function randomly selects a song from the queue and displays it in the provided Discord channel.
    /// Then removes the given song from the queue and enables song guessing til the next day.
    /// </remarks>
    public Task Execute(IJobExecutionContext context)
    {
        try {
            // Iterate all players who guessed successfully and add 2 points to them.
            // Also add 1 point for each player who queued this song.
            foreach (var player in _storageService.Guesses.Where(p => p.Value == true))
            {
                _storageService.AddPoints(_storageService.CurrentUserId, 1);
                _storageService.AddPoints(player.Key, 2);
            }

            // Clear the stored guesses.
            _storageService.Guesses = new();

            // Pop today's song from the queue.
            var song = _storageService.PopRandom();
            // Send the embed to the channel.
            _discordService.SendEmbed(_configuration.DailySongChannelId, song.BuildEmbed());
            // Set the current queuer to the user who queued the song.
            _storageService.CurrentUserId = song.UserId;
        } catch (EmptyQueueException)
        {
            // The queue is empty - send error embed.
            var embed = new EmbedBuilder()
                .WithTitle("Queue is empty!")
                .WithColor(Color.Red)
                .WithDescription("❌ The queue is empty. You can queue a new song using the `/queue` command.");
            _discordService.SendEmbed(_configuration.DailySongChannelId, embed.Build());

            // Update today's user id to 0.
            _storageService.CurrentUserId = 0;
        }

        return Task.CompletedTask;
    }
}