using System.Collections.Generic;
using TradingCardGame.Models.Posts;

namespace TradingCardGame.Services
{
    public interface IPostService
    {
        ICollection<PostViewModel> GetChannelPosts(string channelId);
    }
}
