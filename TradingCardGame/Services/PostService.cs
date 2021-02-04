using System.Linq;
using TradingCardGame.Data;
using System.Collections.Generic;
using TradingCardGame.Models.Posts;
using Microsoft.EntityFrameworkCore;

namespace TradingCardGame.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext context;

        public PostService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public ICollection<PostViewModel> GetChannelPosts(string channelId)
        {
            return this.context.Posts
                .Where(post => post.ChannelId == channelId)
                .Include(post => post.Creator)
                .Select(post => new PostViewModel
                {
                    Id = post.Id,
                    Content = post.Content,
                    CreatedOn = post.CreatedOn,
                    Creator = post.Creator.Email,
                    Score = post.Score
                })
                .ToList();
        }
    }
}
