using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingCardGame.Data.Models
{
    public class Deck
    {
        public Deck()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Cards = new List<DeckCard>();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string ChannelId { get; set; }

        public Channel Channel { get; set; }

        public ICollection<DeckCard> Cards { get; set; }
    }
}
