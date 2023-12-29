using Discord;

namespace Songster.Lib.Commands;

/// <summary>
/// Static class containing the bot command definitions.
/// </summary>
public static class CommandDefinition {
    
    public static SlashCommandBuilder AboutCommand => new SlashCommandBuilder()
        .WithName("about")
        .WithDescription("About the Songster bot");

    public static SlashCommandBuilder QueueCommand => new SlashCommandBuilder()
        .WithName("queue")
        .WithDescription("Queue a new banger to daily playlist")
        .AddOption("link", ApplicationCommandOptionType.String, "Link to YouTube song video", isRequired: true);

    public static SlashCommandBuilder GuessCommand => new SlashCommandBuilder()
        .WithName("guess")
        .WithDescription("Guess who queued today's banger")
        .AddOption("user", ApplicationCommandOptionType.User, "Who do you think queued today's banger?", isRequired: true);

    public static SlashCommandBuilder LeaderboardCommand => new SlashCommandBuilder()
        .WithName("leaderboard")
        .WithDescription("Display's the Songster leaderboard");
}