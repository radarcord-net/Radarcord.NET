using System;
using System.Threading.Tasks;

using DSharpPlus;
using Radarcord.Enums;
using Radarcord.EventArgs;

namespace Radarcord;

public class RadarcordAutomater
{
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

    protected virtual void OnPostReceived(PostEventArgs e)
    {
        PostReceived?.Invoke(this, e);
    }

    protected virtual void OnReviewsReceived(ReviewsEventArgs e)
    {
        ReviewsReceived?.Invoke(this, e);
    }

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

    public RadarcordAutomater(DiscordClient discord, string authorization)
    {
        Discord = discord;
        Authorization = authorization;
        _backend = new(discord, authorization);
    }

    public async Task AutopostStatsAsync(int shardCount = 1, IntervalPreset preset = IntervalPreset.Default)
    {
        int interval = IntervalPresetMethods.GetInterval(preset);
        await PostStatsAsync(shardCount);
        await Task.Delay(interval * 1000);
        await AutopostStatsAsync(shardCount, preset);
    }

    public async Task AutoGetReviewsAsync(IntervalPreset preset = IntervalPreset.Default)
    {
        int interval = IntervalPresetMethods.GetInterval(preset);
        await GetReviewsAsync();
        await Task.Delay(interval * 1000);
        await AutoGetReviewsAsync(preset);
    }
}
