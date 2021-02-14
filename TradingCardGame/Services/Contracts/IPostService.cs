using System.Threading.Tasks;
using System.Collections.Generic;

using TradingCardGame.Models.Posts;

namespace TradingCardGame.Services.Contracts
{
    public interface IPostService
    {
        ICollection<PostViewModel> GetChannelPosts(string channelId, string userId);

        Task<PostViewModel> CreateAsync(string channelId, string userId, string content);

        PostViewModel GetPostById(string postId, string userId);

        Task Vote(string postId, string userId);
    }
}
