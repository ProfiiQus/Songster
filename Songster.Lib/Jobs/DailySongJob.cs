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
class DailySongJob : IJob
{
    /// <summary>
    /// Bot service.
    /// </summary>
    public BotService _bot { get; set;}

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
    public DailySongJob(IOptions<BotConfiguration> configuration, BotService bot, StorageService storageService)
    {
        _configuration = configuration.Value;
        _bot = bot;
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
        Console.WriteLine("Running daily");
        try {
            // Pop today's song from the queue.
            var song = _storageService.PopRandom();
            // Send the embed to the channel.
            _bot.SendEmbed(_configuration.DailySongChannelId, song.BuildEmbed());
            // Set the current queuer to the user who queued the song.
            _configuration.CurrentQueuerId = song.UserId;
        } catch (EmptyQueueException)
        {
            // The queue is empty - send error embed.
            var embed = new EmbedBuilder()
                .WithTitle("Queue is empty!")
                .WithColor(Color.Red)
                .WithDescription("❌ The queue is empty. You can queue a new song using the `/queue` command.");

            _bot.SendEmbed(_configuration.DailySongChannelId, embed.Build());
        }

        return Task.CompletedTask;
    }
}