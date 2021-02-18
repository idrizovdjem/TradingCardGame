using System;
using TradingCardGame.Enums;
using System.ComponentModel.DataAnnotations;

namespace TradingCardGame.Data.Models
{
    public class Card
    {
        public Card()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Description { get; set; }

        public CardType Type { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        [Required]
        public string ChannelId { get; set; }

        public Channel Channel { get; set; }

        public CardStatus Status { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }
    }
}
