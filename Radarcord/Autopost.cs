using System;
using System.Threading.Tasks;

using DSharpPlus;
using Radarcord.Enums;
using Radarcord.EventArgs;

namespace Radarcord;

public class RadarcordAutomater
{
    #region Fields
    /// <summary>
    /// Your DSharpPlus client, NOT SETTABLE.
    /// </summary>
    public DiscordClient Discord { get; private set; }
    /// <summary>
    /// Your Radarcord API Token.
    /// </summary>
    public string Authorization { get; set; }
    /// <summary>
    /// Fired every time a post result is received.
    /// </summary>
    public event EventHandler<PostEventArgs> PostReceived;
    /// <summary>
    /// Fired every time some reviews are received from the API.
    /// </summary>
    public event EventHandler<ReviewsEventArgs> ReviewsReceived;
    private readonly RadarcordClient _backend;
    #endregion

    #region Protected Methods
    protected virtual void OnPostReceived(PostEventArgs e)
    {
        PostReceived?.Invoke(this, e);
    }

    protected virtual void OnReviewsReceived(ReviewsEventArgs e)
    {
        ReviewsReceived?.Invoke(this, e);
    }
    #endregion

    #region Helper Methods
    private async Task PostStatsAsync(int shardCount)
    {
        var result = await _backend.PostStatsAsync(shardCount);
        OnPostReceived(new PostEventArgs(result));
    }

    private async Task GetReviewsAsync()
    {
        var reviews = await _backend.GetReviewsAsync();
        OnReviewsReceived(new ReviewsEventArgs(reviews));
    }
    #endregion

    /// <summary>
    /// Creates a new instance of the RadarcordAutomater class
    /// </summary>
    /// <param name="discordClient">Your DSharpPlus Client</param>
    /// <param name="authorization">Your Radarcord API Token</param>
    public RadarcordAutomater(DiscordClient discordClient, string authorization)
    {
        Discord = discordClient;
        Authorization = authorization;
        _backend = new(discordClient, authorization);
    }

    #region Public Methods
    /// <summary>
    /// Automatically posts stats to the Radarcord API.
    /// </summary>
    /// <param name="shardCount">How many shards your bot has, if any.</param>
    /// <param name="preset">How long between requests to pause</param>
    public async Task AutopostStatsAsync(int shardCount = 1, IntervalPreset preset = IntervalPreset.Default)
    {
        int interval = IntervalPresetMethods.GetInterval(preset);
        await PostStatsAsync(shardCount);
        await Task.Delay(interval * 1000);
        await AutopostStatsAsync(shardCount, preset);
    }

    /// <summary>
    /// Automatically grabs reviews from the Radarcord API.
    /// </summary>
    /// <param name="preset">How long between requests to pause</param>
    public async Task AutoGetReviewsAsync(IntervalPreset preset = IntervalPreset.Default)
    {
        int interval = IntervalPresetMethods.GetInterval(preset);
        await GetReviewsAsync();
        await Task.Delay(interval * 1000);
        await AutoGetReviewsAsync(preset);
    }
    #endregion
}
