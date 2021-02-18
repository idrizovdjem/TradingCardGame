using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using TradingCardGame.Data;
using TradingCardGame.Enums;
using TradingCardGame.Data.Models;
using TradingCardGame.Models.Browse;
using TradingCardGame.Models.Channel;
using TradingCardGame.Models.Account;
using TradingCardGame.Services.Contracts;

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

        public async Task AddUserToChannelAsync(string userId, string channelId)
        {
            var channel = this.context.Channels
                .FirstOrDefault(x => x.Id == channelId && x.IsDeleted == false);

            if(channel == null)
            {
                return;
            }

            if(this.context.UserChannels
                .Any(x => x.ChannelId == channelId && x.UserId == userId))
            {
                return;
            }

            var userChannel = new UserChannels()
            {
                UserId = userId,
                ChannelId = channelId,
            };

            if(this.IsUserOwner(channel.Name, userId))
            {
                userChannel.Role = ChannelUserRole.Administrator;
            }
            else
            {
                userChannel.Role = ChannelUserRole.User;
            }

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
                .Any(c => c.CreatorId == userId && c.IsDeleted == false);
        }

        public bool IsChannelNameAvailable(string channelName)
        {
            return !this.context.Channels
                .Any(ch => ch.Name == channelName);
        }

        public bool IsUserOwner(string channelName, string userId)
        {
            return this.context.Channels
                .Any(ch => ch.Name == channelName && ch.CreatorId == userId && ch.IsDeleted == false);
        }

        public IEnumerable<BrowseChannelViewModel> GetTopTenChannels(string userId)
        {
            var channels =  this.context.Channels
                .Where(ch => ch.IsDeleted == false)
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
                .Where(ch => ch.Name.Contains(name) && ch.IsDeleted == false)
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

        public async Task RemoveUserFromChannelAsync(string channelName, string userId)
        {
            var channelId = this.GetChannelIdByName(channelName);

            var userChannel = this.context.UserChannels
                .FirstOrDefault(ch => ch.ChannelId == channelId && ch.UserId == userId);

            if(userChannel == null)
            {
                return;
            }

            this.context.UserChannels.Remove(userChannel);

            var userChannelsCount = this.context.UserChannels
                .Count(ch => ch.ChannelId == channelId) - 1;

            if(userChannelsCount == 0)
            {
                var channel = this.context.Channels
                    .FirstOrDefault(ch => ch.Id == channelId);

                if(channel != null)
                {
                    channel.IsDeleted = true;
                }
            }

            await this.context.SaveChangesAsync();
        }

        public ChannelInformationViewModel GetChannelInformation(string channelName)
        {
            var channel = this.context.Channels
                .Where(ch => ch.Name == channelName)
                .Select(x => new ChannelInformationViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    CurrentPlayers = this.context.UserChannels
                        .Count(ch => ch.ChannelId == x.Id),
                    MaxPlayers = x.MaxUsers,
                    PostsCount = this.context.Posts
                        .Count(p => p.ChannelId == x.Id && p.IsDeleted == false),
                    CardsCount = this.context.Cards
                        .Count(c => c.ChannelId == x.Id && c.Status == CardStatus.Approved)
                })
                .FirstOrDefault();

            var channelPosts = this.context.Posts
                .Where(p => p.ChannelId == channel.Id && p.IsDeleted == false)
                .ToList();

            var totalLikes = 0;
            foreach(var post in channelPosts)
            {
                var postLikes = this.context.PostVotes
                    .Count(pv => pv.PostId == post.Id && pv.IsDeleted == false);
                totalLikes += postLikes;
            }

            channel.TotalLikes = totalLikes;

            return channel;
        }

        public async Task UpdateChannelAsync(CreateChannelInputModel input, string userId)
        {
            var userChannel = this.context.Channels
                .FirstOrDefault(x => x.CreatorId == userId);

            if(userChannel == null)
            {
                return;
            }

            userChannel.Name = input.Name;
            userChannel.MaxUsers = input.MaxPlayers;
            userChannel.Security = input.Security;

            await this.context.SaveChangesAsync();
        }

        public IEnumerable<UserChannelViewModel> GetChannelUsers(string userId)
        {
            var channelId = this.context.Channels
                 .Where(x => x.CreatorId == userId && x.IsDeleted == false)
                 .Select(x => x.Id)
                 .FirstOrDefault();

            if(channelId == null)
            {
                return null;
            }

            var activeUsers = this.context.UserChannels
                .Where(x => x.ChannelId == channelId && x.UserId != userId)
                .Select(x => x.UserId)
                .ToList();

            var users = new List<UserChannelViewModel>();
            foreach(var activeId in activeUsers)
            {
                var user = this.context.Users
                    .Where(x => x.Id == activeId)
                    .Select(x => new UserChannelViewModel()
                    {
                        Id = x.Id,
                        Email = x.Email
                    })
                    .FirstOrDefault();

                user.Role = this.context.UserChannels
                    .Where(x => x.UserId == user.Id && x.ChannelId == channelId)
                    .Select(x => x.Role.ToString())
                    .FirstOrDefault();

                users.Add(user);
            }

            return users;
        }

        public string GetChannelName(string userId)
        {
            return this.context.Channels
                .Where(x => x.CreatorId == userId && x.IsDeleted == false)
                .Select(x => x.Name)
                .FirstOrDefault();
        }

        public string GetUserRole(string channelName, string userId)
        {
            var channelId = this.GetChannelIdByName(channelName);
            return this.context.UserChannels
                .Where(x => x.ChannelId == channelId && x.UserId == userId)
                .Select(x => x.Role.ToString())
                .FirstOrDefault();
        }

        public async Task AddUserToRoleAsync(string creatorId, string userId, ChannelUserRole role)
        {
            var channelId = this.context.Channels
                .Where(x => x.CreatorId == creatorId && x.IsDeleted == false)
                .Select(x => x.Id)
                .FirstOrDefault();

            if(channelId == null)
            {
                return;
            }

            var userChannel = this.context.UserChannels
                .FirstOrDefault(x => x.UserId == userId && x.ChannelId == channelId);
            userChannel.Role = role;

            await this.context.SaveChangesAsync();
        }
    }
}
