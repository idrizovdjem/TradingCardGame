using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using TradingCardGame.Data;
using TradingCardGame.Data.Enums;
using TradingCardGame.Models.Card;
using TradingCardGame.Data.Models;
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

        public async Task CreateAsync(CreateCardInputModel input, string userId)
        {
            var channel = this.context.Channels
                .FirstOrDefault(x => x.Name == input.ChannelName);

            if(channel == null)
            {
                return;
            }

            var userChannel = this.context.UserChannels
                .FirstOrDefault(x => x.ChannelId == channel.Id && x.UserId == userId);

            if(userChannel == null)
            {
                return;
            }

            var card = new Card()
            {
                Name = input.Name,
                Type = input.Type,
                Image = input.Image,
                Description = input.Description,
                ChannelId = channel.Id,
                Attack = input.Attack,
                Defense = input.Defense,
                CreatorId = userId,
            };

            if(userChannel.Role == ChannelUserRole.User)
            {
                card.Status = CardStatus.ForReview;
            }
            else
            {
                card.Status = CardStatus.Approved;
            }

            await this.context.Cards.AddAsync(card);
            await this.context.SaveChangesAsync();
        }

        public EditCardInputModel GetCardForEdit(string cardId)
        {
            return this.context.Cards
                .Where(x => x.Id == cardId)
                .Include(x => x.Channel)
                .Select(x => new EditCardInputModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Type = x.Type,
                    Image = x.Image,
                    Description = x.Description,
                    Attack = x.Attack,
                    Defense = x.Defense,
                    ChannelName = x.Channel.Name
                })
                .FirstOrDefault();
        }

        public CardViewModel GetCardForReview(string cardId)
        {
            return this.context.Cards
                .Where(x => x.Id == cardId && (x.Status == CardStatus.ForReview || x.Status == CardStatus.Archived))
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
                .FirstOrDefault();
        }

        public IEnumerable<CardViewModel> GetCardsWithStatus(string channelId, CardStatus status)
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
                ApprovedCards = this.GetCardsWithStatus(channel.Id, CardStatus.Approved)
            };

            return cardsModel;
        }

        public async Task ReviewCard(string cardId, CardStatus status)
        {
            var card = this.context.Cards
                .FirstOrDefault(x => x.Id == cardId);

            if(card == null)
            {
                return;
            }

            if(card.Status == CardStatus.Archived && status == CardStatus.Archived)
            {
                status = CardStatus.Deleted;
            }

            card.Status = status;

            await this.context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EditCardInputModel input)
        {
            var card = this.context.Cards
                .FirstOrDefault(x => x.Id == input.Id);
            
            if(card == null)
            {
                return;
            }

            card.Name = input.Name;
            card.Image = input.Image;
            card.Description = input.Description;
            card.Type = input.Type;
            card.Attack = input.Attack;
            card.Defense = input.Defense;

            await this.context.SaveChangesAsync();
        }
    }
}
