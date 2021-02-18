using System.ComponentModel.DataAnnotations;

namespace TradingCardGame.Models.Deck
{
    public class CreateDeckInputModel
    {
        [Required]
        [MinLength(2), MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string ChannelName { get; set; }
    }
}
