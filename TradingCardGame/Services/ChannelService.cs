using System.Linq;
using TradingCardGame.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using TradingCardGame.Data.Enums;
using TradingCardGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using TradingCardGame.Models.Channel;
using TradingCardGame.Models.Browse;
using TradingCardGame.Models.Enums;

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

        public async Task AddUserToChannelAsync(string userId, string channelId, ChannelUserRole role)
        {
            if(!this.context.Channels.Any(x => x.Id == channelId))
            {
                return;
            }

            var userChannel = new UserChannels()
            {
                UserId = userId,
                ChannelId = channelId,
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

        public IEnumerable<BrowseChannelViewModel> GetTopTenChannels(string userId)
        {
            var channels =  this.context.Channels
                .Select(x => new BrowseChannelViewModel()
                {
                    Id = x.Id,
                    MaxPlayers = x.MaxUsers,
                    Name = x.Name,
                    CurrentPlayers = this.context.UserChannels
                        .Count(ch => ch.ChannelId == x.Id),
                })
                .OrderByDescending(x => x.CurrentPlayers)
                .Take(10)
                .ToList();

            foreach(var channel in channels)
            {
                channel.Status = this.GetChannelStatus(userId, channel.Id);
            }

            return channels;
        }

        public ChannelStatus GetChannelStatus(string userId, string channelId)
        {
            if(this.context.UserChannels
                .Any(x => x.ChannelId == channelId && x.UserId == userId))
            {
                return ChannelStatus.Joined;
            }

            if(this.context.Channels
                .FirstOrDefault(ch => ch.Id == channelId)?.Security == ChannelType.Private)
            {
                return ChannelStatus.Private;
            }

            return ChannelStatus.Available;
        }

        public IEnumerable<BrowseChannelViewModel> GetChannelsContainingName(string name, string userId)
        {
            var channels = this.context.Channels
                .Where(ch => ch.Name.Contains(name))
                .Select(x => new BrowseChannelViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    CurrentPlayers = this.context.UserChannels
                        .Count(ch => ch.ChannelId == x.Id),
                    MaxPlayers = x.MaxUsers
                })
                .ToList();

            foreach(var channel in channels)
            {
                channel.Status = this.GetChannelStatus(userId, channel.Id);
            }

            return channels;
        }
    }
}
