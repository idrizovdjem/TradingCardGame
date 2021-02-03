using System.Linq;
using TradingCardGame.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using TradingCardGame.Data.Models;
using TradingCardGame.Models.Channel;
using TradingCardGame.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace TradingCardGame.Services
{
    public class ChannelService : IChannelService
    {
        private readonly ApplicationDbContext context;

        public ChannelService(ApplicationDbContext context)
        {
            this.context = context;
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

        public async Task CreateAsync(string channelName, string userId)
        {
            var channel = new Channel()
            {
                Name = channelName,
                CreatorId = userId,
                IsDeleted = false
            };

            await this.context.Channels.AddAsync(channel);
            await this.context.SaveChangesAsync();
        }

        public bool Exists(string channelName)
        {
            return this.context.Channels.Any(channel => channel.Name == channelName);
        }

        public ChannelViewModel GetChannelByName(string channelName)
        {
            var channel = new ChannelViewModel()
            {
                Name = channelName
                // add channel content
            };
            
            return channel;
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
    }
}
