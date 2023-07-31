// MIT License
//
// Copyright (c) 2023 Yoshiboi18303
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using DSharpPlus;
using Newtonsoft.Json;
using Radarcord.Errors;
using Radarcord.Logging;
using Radarcord.Types;

namespace Radarcord;

public class RadarcordClient
{
    /// <summary>
    /// Your DSharpPlus client, NOT SETTABLE.
    /// </summary>
    public DiscordClient Discord { get; private set; }
    /// <summary>
    /// Your Radarcord API Token.
    /// </summary>
    public string Authorization { get; set; }
    private string _apiBase { get; } = "https://radarcord.net/api";

    /// <summary>
    /// Creates a new instance of the RadarcordClient class
    /// </summary>
    /// <param name="discordClient">Your DSharpPlus Client</param>
    /// <param name="authorization">Your Radarcord API Token</param>
    public RadarcordClient(DiscordClient discordClient, string authorization)
    {
        Discord = discordClient;
        Authorization = authorization;
    }

    /// <summary>
    /// Posts your bot's stats to the Radarcord API.
    /// </summary>
    /// <param name="shardCount">How many shards your bot has, if any. Defaults to 1.</param>
    /// <returns>A PostResult with the status code of the post as well as the message from the API.</returns>
    /// <exception cref="RadarcordException"></exception>
    public async Task<PostResult> PostStatsAsync(int shardCount = 1)
    {
        using var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("Authorization", Authorization);

        var payload = new Dictionary<string, int>
        {
            { "guilds", Discord.Guilds.Count },
            { "shards", shardCount }
        };
        var content = new StringContent(JsonConvert.SerializeObject(payload));

        try
        {
            using var response = await httpClient.PostAsync($"{_apiBase}/bot/{Discord.CurrentUser.Id}/stats", content);
            int statusCode = Convert.ToInt16(response.StatusCode);
            var body = JsonConvert.DeserializeObject<BaseStatsResponse>(response.Content.ReadAsStringAsync().Result);
            PostResult result = new(statusCode, body.Message);

            if (response.IsSuccessStatusCode)
            {
                httpClient.Dispose();
                return result;
            }
            else
            {
                // Example message: "Failed to post, the status code returned is 404. Try again, you got this."
                Logger.Error($"Failed to post, the status code returned is {statusCode}. Try again, you got this.");
                httpClient.Dispose();
                return result;
            }
        }
        catch (Exception err)
        {
            httpClient.Dispose();
            throw new RadarcordException($"[RADARCORD] An exception occurred.\n{err}");
        }
    }

    public async Task<ReadOnlyCollection<Review>> GetReviewsAsync()
    {
        using var httpClient = new HttpClient();

        try
        {
            using var response = await httpClient.GetAsync($"{_apiBase}/bot/{Discord.CurrentUser.Id}/reviews");
            int statusCode = Convert.ToInt16(response.StatusCode);
            List<Review> reviews = new();

            if (response.IsSuccessStatusCode)
            {
                var body = JsonConvert.DeserializeObject<BaseReviewsResponse>(response.Content.ReadAsStringAsync().Result);
                var apiReviews = body.Reviews;

                foreach (BaseReview review in apiReviews)
                {
                    reviews.Add(new Review(review.content, Convert.ToInt16(review.stars), review.userid, review.botid));
                }

                return new ReadOnlyCollection<Review>(reviews);
            }
            else
            {
                Logger.Error($"Failed to get reviews, the status code returned was {statusCode}. Try again, you got this.");
                httpClient.Dispose();
                return new ReadOnlyCollection<Review>(reviews);
            }
        }
        catch (Exception err)
        {
            httpClient.Dispose();
            throw new RadarcordException($"[RADARCORD] An exception occurred.\n{err}");
        }
    }
}
