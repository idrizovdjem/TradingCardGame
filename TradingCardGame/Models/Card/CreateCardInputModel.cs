using System.ComponentModel.DataAnnotations;

using TradingCardGame.Data.Enums;

namespace TradingCardGame.Models.Card
{
    public class CreateCardInputModel
    {
        [Required]
        public string ChannelName { get; set; }

        [Required]
        [MinLength(2), MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        public CardType Type { get; set; }

        [Required]
        public string Description { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }
    }
}
