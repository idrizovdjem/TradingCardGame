using System.Threading.Tasks;
using TradingCardGame.Data.Enums;
using System.Collections.Generic;
using TradingCardGame.Data.Models;
using TradingCardGame.Models.Channel;
using TradingCardGame.Models.Browse;
using TradingCardGame.Models.Account;

namespace TradingCardGame.Services
{
    public interface IChannelService
    {
        Task CreateAsync(Channel input);

        Task AddUserToChannelAsync(string userId, string channelId, ChannelUserRole role);

        string GetChannelIdByName(string channelName);

        string GetChannelName(string userId);

        List<string> GetUserChannels(string userId);

        ChannelViewModel GetChannelContent(string channelName, string userId);

        bool Exists(string channelName);

        bool HaveUserCreatedChannel(string userId);

        bool IsChannelNameAvailable(string channelName);

        bool IsUserOwner(string channelName, string userId);

        IEnumerable<BrowseChannelViewModel> GetTopTenChannels(string userId);

        IEnumerable<BrowseChannelViewModel> GetChannelsContainingName(string name, string userId);

        Task RemoveUserFromChannelAsync(string channelName, string userId);

        Task UpdateChannelAsync(CreateChannelInputModel input, string userId);

        Task AddUserToRoleAsync(string creatorId, string userId, ChannelUserRole role);

        ChannelInformationViewModel GetChannelInformation(string channelName);

        IEnumerable<UserChannelViewModel> GetChannelUsers(string userId);
    }
}
