using System.Threading.Tasks;
using TradingCardGame.Data.Enums;
using System.Collections.Generic;
using TradingCardGame.Models.Channel;

namespace TradingCardGame.Services
{
    public interface IChannelService
    {
        bool Exists(string channelName);

        Task CreateAsync(string channelName, string userId);

        List<string> GetUserChannels(string userId);

        ChannelViewModel GetChannelByName(string channelName);

        Task AddUserToChannel(string userId, string channelName, ChannelUserRole role);

        ChannelViewModel GetChannelContent(string channelName);

        string GetChannelIdByName(string channelName);
    }
}
