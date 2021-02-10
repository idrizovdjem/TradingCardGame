﻿using System.Threading.Tasks;
using TradingCardGame.Data.Enums;
using System.Collections.Generic;
using TradingCardGame.Data.Models;
using TradingCardGame.Models.Channel;
using TradingCardGame.Models.Browse;

namespace TradingCardGame.Services
{
    public interface IChannelService
    {
        Task CreateAsync(Channel input);

        Task AddUserToChannelAsync(string userId, string channelId, ChannelUserRole role);

        string GetChannelIdByName(string channelName);

        List<string> GetUserChannels(string userId);

        ChannelViewModel GetChannelContent(string channelName, string userId);

        bool Exists(string channelName);

        bool HaveUserCreatedChannel(string userId);

        bool IsChannelNameAvailable(string channelName);

        IEnumerable<BrowseChannelViewModel> GetTopTenChannels(string userId);
    }
}
