using System.Linq;
using System.Collections.Generic;

using TradingCardGame.Data;
using TradingCardGame.Data.Enums;
using TradingCardGame.Models.Card;
using TradingCardGame.Services.Contracts;

namespace TradingCardGame.Services
{
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext context;

        public CardService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<CardViewModel> GetCards(string channelId, CardStatus status)
        {
            return this.context.Cards
                .Where(x => x.ChannelId == channelId && x.Status == status)
                .Select(x => new CardViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Image = x.Image,
                    Description = x.Description,
                    Type = x.Type.ToString(),
                    Attack = x.Attack,
                    Defense = x.Defense
                })
                .ToList();
        }

        public ChannelCardsViewModel GetChannelCards(string channelName, string userId)
        {
            var channel = this.context.Channels
                .FirstOrDefault(x => x.Name == channelName);

            if(channel == null)
            {
                return null;
            }

            var userChannel = this.context.UserChannels
                .FirstOrDefault(x => x.ChannelId == channel.Id && x.UserId == userId);

            if(userChannel == null)
            {
                return null;
            }

            var cardsModel = new ChannelCardsViewModel()
            {
                UserRole = userChannel.Role.ToString(),
                ApprovedCards = this.GetCards(channel.Id, CardStatus.Approved)
            };

            return cardsModel;
        }
    }
}
