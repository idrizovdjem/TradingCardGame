using System.ComponentModel.DataAnnotations;

using TradingCardGame.Enums;

namespace TradingCardGame.Models.Channel
{
    public class CreateChannelInputModel
    {
        [Required]
        public string Name { get; set; }

        public ChannelType Security { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxPlayers { get; set; }
    }
}
