using System.Threading.Tasks;
using System.Collections.Generic;

using TradingCardGame.Models.Deck;

namespace TradingCardGame.Services.Contracts
{
    public interface IDeckService
    {
        IEnumerable<string> GetUserDecks(string userId, string channelName);

        Task CreateAsync(CreateDeckInputModel input, string userId);
    }
}
