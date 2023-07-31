using System.Collections.ObjectModel;
using Radarcord.Types;

namespace Radarcord.EventArgs;

public class PostEventArgs : System.EventArgs
{
    public PostResult Result { get; }

    public PostEventArgs(PostResult result)
    {
        Result = result;
    }
}

public class ReviewsEventArgs : System.EventArgs
{
    public ReadOnlyCollection<Review> Reviews { get; }

    public ReviewsEventArgs(ReadOnlyCollection<Review> reviews)
    {
        Reviews = reviews;
    }
}
