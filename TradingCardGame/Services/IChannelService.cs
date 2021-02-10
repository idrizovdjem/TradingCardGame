using System.Threading.Tasks;
using TradingCardGame.Data.Enums;
using System.Collections.Generic;
using TradingCardGame.Data.Models;
using TradingCardGame.Models.Channel;

namespace TradingCardGame.Services
{
    public interface IChannelService
    {
        Task CreateAsync(Channel input);

        Task AddUserToChannel(string userId, string channelName, ChannelUserRole role);

        string GetChannelIdByName(string channelName);

        List<string> GetUserChannels(string userId);

        ChannelViewModel GetChannelContent(string channelName, string userId);

        bool Exists(string channelName);

        bool HaveUserCreatedChannel(string userId);

        bool IsChannelNameAvailable(string channelName);
    }
}
