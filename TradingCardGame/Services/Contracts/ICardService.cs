using System.Collections.Generic;

using TradingCardGame.Data.Enums;
using TradingCardGame.Models.Card;

namespace TradingCardGame.Services.Contracts
{
    public interface ICardService
    {
        ChannelCardsViewModel GetChannelCards(string channelName, string userId);

        IEnumerable<CardViewModel> GetCards(string channelId, CardStatus status);
    }
}
