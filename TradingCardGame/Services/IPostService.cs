using System.Threading.Tasks;
using System.Collections.Generic;
using TradingCardGame.Models.Posts;

namespace TradingCardGame.Services
{
    public interface IPostService
    {
        ICollection<PostViewModel> GetChannelPosts(string channelId);

        Task<PostViewModel> CreateAsync(string channelId, string userId, string content);

        PostViewModel GetPostById(string postId);
    }
}
