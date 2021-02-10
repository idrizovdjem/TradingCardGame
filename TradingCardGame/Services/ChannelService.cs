using System.Linq;
using TradingCardGame.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using TradingCardGame.Data.Enums;
using TradingCardGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using TradingCardGame.Models.Channel;

namespace TradingCardGame.Services
{
    public class ChannelService : IChannelService
    {
        private readonly ApplicationDbContext context;
        private readonly IPostService postService;

        public ChannelService(ApplicationDbContext context, IPostService postService)
        {
            this.context = context;
            this.postService = postService;
        }

        public async Task AddUserToChannel(string userId, string channelName, ChannelUserRole role)
        {
            var channel = this.context.Channels
                .FirstOrDefault(ch => ch.Name == channelName);
            if(channel == null)
            {
                return;
            }

            var userChannel = new UserChannels()
            {
                UserId = userId,
                ChannelId = channel.Id,
                Role = role
            };

            await this.context.UserChannels.AddAsync(userChannel);
            await this.context.SaveChangesAsync();
        }

        public async Task CreateAsync(Channel channel)
        {
            await this.context.Channels.AddAsync(channel);
            await this.context.SaveChangesAsync();
        }

        public ChannelViewModel GetChannelContent(string channelName, string userId)
        {
            var channelId = this.GetChannelIdByName(channelName);
            if(channelId == null)
            {
                return null;
            }

            var channel = new ChannelViewModel()
            {
                Name = channelName,
                Posts = this.postService.GetChannelPosts(channelId, userId)
            };

            return channel;
        }

        public string GetChannelIdByName(string channelName)
        {
            return this.context.Channels
                .Where(c => c.Name == channelName)
                .Select(c => c.Id)
                .FirstOrDefault();
        }

        public List<string> GetUserChannels(string userId)
        {
            var user = this.context.Users
                .Include(user => user.Channels)
                .ThenInclude(uc => uc.Channel)
                .FirstOrDefault(user => user.Id == userId);

            if (user == null)
            {
                return null;
            }

            return user.Channels
                .Select(uc => uc.Channel.Name)
                .ToList();
        }

        public bool Exists(string channelName)
        {
            return this.context.Channels.Any(channel => channel.Name == channelName);
        }

        public bool HaveUserCreatedChannel(string userId)
        {
            return this.context.Channels
                .Any(c => c.CreatorId == userId);
        }

        public bool IsChannelNameAvailable(string channelName)
        {
            return !this.context.Channels
                .Any(ch => ch.Name == channelName);
        }
    }
}
