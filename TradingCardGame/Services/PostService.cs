using System;
using System.Linq;
using TradingCardGame.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using TradingCardGame.Data.Models;
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

        public async Task<PostViewModel> CreateAsync(string channelId, string userId, string content)
        {
            var post = new Post()
            {
                CreatorId = userId,
                ChannelId = channelId,
                Content = content,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Score = 0
            };

            await this.context.Posts.AddAsync(post);
            await this.context.SaveChangesAsync();

            return this.GetPostById(post.Id);
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
                    CreatedOn = post.CreatedOn.ToString("d"),
                    Creator = post.Creator.Email,
                    Score = post.Score
                })
                .ToList();
        }

        public PostViewModel GetPostById(string postId)
        {
            return this.context.Posts
                .Where(post => post.Id == postId)
                .Include(post => post.Creator)
                .Select(post => new PostViewModel()
                {
                    Content = post.Content,
                    CreatedOn = post.CreatedOn.ToString("d"),
                    Creator = post.Creator.Email,
                    Score = post.Score
                })
                .FirstOrDefault();

        }
    }
}
