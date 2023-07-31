using System.Collections.Generic;

namespace Radarcord.Types;

internal class BaseStatsResponse
{
    public string Message { get; set; }
}

// The properties are intentionally named incorrectly to match the Radarcord API.
internal struct BaseReview
{
    public string content { get; set; }
    public string stars { get; set; }
    public string userid { get; set; }
    public string botid { get; set; }
}

internal class BaseReviewsResponse
{
    public List<BaseReview> Reviews { get; set; }
}

public struct PostResult
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public PostResult(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}

public struct Review
{
    public string Content { get; private set; }
    public int Stars { get; private set; }
    public string UserId { get; private set; }
    public string BotId { get; private set; }

    internal Review(string content, int stars, string userId, string botId)
    {
        Content = content;
        Stars = stars;
        UserId = userId;
        BotId = botId;
    }
}
