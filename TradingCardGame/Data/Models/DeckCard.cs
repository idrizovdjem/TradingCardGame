using System.ComponentModel.DataAnnotations;

namespace TradingCardGame.Data.Models
{
    public class DeckCard
    {
        public int Id { get; set; }

        [Required]
        public string DeckId { get; set; }

        public Deck Deck { get; set; }

        [Required]
        public string CardId { get; set; }

        public Card Card { get; set; }
    }
}
