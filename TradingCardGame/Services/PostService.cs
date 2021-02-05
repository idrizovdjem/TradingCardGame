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
                IsDeleted = false
            };

            await this.context.Posts.AddAsync(post);
            await this.context.SaveChangesAsync();

            return this.GetPostById(post.Id, userId);
        }

        public ICollection<PostViewModel> GetChannelPosts(string channelId, string userId)
        {
            return this.context.Posts
                .Where(post => post.ChannelId == channelId)
                .Include(post => post.Creator)
                .OrderByDescending(p => p.CreatedOn)
                .Select(post => new PostViewModel
                {
                    Id = post.Id,
                    Content = post.Content,
                    CreatedOn = post.CreatedOn.ToString("d"),
                    Creator = post.Creator.Email,
                    Score = this.context.PostVotes
                        .Count(pv => pv.PostId == post.Id && pv.UserId == userId && pv.IsDeleted == false),
                    IsVoted = this.context.PostVotes
                        .Any(pv => pv.PostId == post.Id && pv.UserId == userId && pv.IsDeleted == false)
                })
                .ToList();
        }

        public PostViewModel GetPostById(string postId, string userId)
        {
            return this.context.Posts
                .Where(post => post.Id == postId)
                .Include(post => post.Creator)
                .Select(post => new PostViewModel()
                {
                    Content = post.Content,
                    CreatedOn = post.CreatedOn.ToString("d"),
                    Creator = post.Creator.Email,
                    Score = this.context.PostVotes
                        .Count(pv => pv.PostId == post.Id && pv.UserId == userId && pv.IsDeleted == false),
                    IsVoted = this.context.PostVotes
                        .Any(pv => pv.PostId == post.Id && pv.UserId == userId && pv.IsDeleted == false)
                })
                .FirstOrDefault();
        }

        public async Task Vote(string postId, string userId)
        {
            var vote = this.context.PostVotes
                .FirstOrDefault(pv => pv.PostId == postId && pv.UserId == userId);
            if(vote == null)
            {
                vote = new PostVote()
                {
                    UserId = userId,
                    PostId = postId,
                    IsDeleted = false
                };

                await this.context.PostVotes.AddAsync(vote);
            }
            else
            {
                vote.IsDeleted = !vote.IsDeleted;
                this.context.PostVotes.Update(vote);
            }

            await this.context.SaveChangesAsync();
        }
    }
}
