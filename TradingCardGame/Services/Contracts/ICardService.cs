using System.Threading.Tasks;
using System.Collections.Generic;

using TradingCardGame.Enums;
using TradingCardGame.Models.Card;

namespace TradingCardGame.Services.Contracts
{
    public interface ICardService
    {
        ChannelCardsViewModel GetChannelCards(string channelName, string userId);

        IEnumerable<CardViewModel> GetCardsWithStatus(string channelId, CardStatus status);

        Task CreateAsync(CreateCardInputModel input, string userId);

        Task UpdateAsync(EditCardInputModel input);

        EditCardInputModel GetCardForEdit(string cardId);

        CardViewModel GetCardForReview(string cardId);

        Task ReviewCard(string cardId, CardStatus status);
    }
}
