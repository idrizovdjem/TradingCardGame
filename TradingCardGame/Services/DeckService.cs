using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using TradingCardGame.Data;
using TradingCardGame.Models.Deck;
using TradingCardGame.Data.Models;
using TradingCardGame.Services.Contracts;

namespace TradingCardGame.Services
{
    public class DeckService : IDeckService
    {
        private readonly ApplicationDbContext context;
        private readonly IChannelService channelService;

        public DeckService(ApplicationDbContext context, IChannelService channelService)
        {
            this.context = context;
            this.channelService = channelService;
        }

        public async Task CreateAsync(CreateDeckInputModel input, string userId)
        {
            var channelId = this.channelService.GetChannelIdByName(input.ChannelName);
            if(channelId == null)
            {
                return;
            }

            if(this.context.Decks
                .Any(x => x.UserId == userId && x.ChannelId == channelId && x.Name == input.Name))
            {
                return;
            }

            var deck = new Deck()
            {
                Name = input.Name,
                ChannelId = channelId,
                UserId = userId
            };

            await this.context.Decks.AddAsync(deck);
            await this.context.SaveChangesAsync();
        }

        public IEnumerable<string> GetUserDecks(string userId, string channelName)
        {
            var channelId = this.channelService.GetChannelIdByName(channelName);
            if(channelId == null)
            {
                return null;
            }

            return this.context.Decks
                .Where(x => x.UserId == userId && x.ChannelId == channelId)
                .Select(x => x.Name)
                .ToList();
        }
    }
}
