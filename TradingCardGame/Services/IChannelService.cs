using System.Threading.Tasks;
using TradingCardGame.Data.Enums;
using System.Collections.Generic;
using TradingCardGame.Data.Models;
using TradingCardGame.Models.Channel;

namespace TradingCardGame.Services
{
    public interface IChannelService
    {
        bool Exists(string channelName);

        Task CreateAsync(Channel input);

        List<string> GetUserChannels(string userId);

        ChannelViewModel GetChannelByName(string channelName);

        Task AddUserToChannel(string userId, string channelName, ChannelUserRole role);

        ChannelViewModel GetChannelContent(string channelName, string userId);

        string GetChannelIdByName(string channelName);

        bool HaveUserCreatedChannel(string userId);
    }
}
