using TradingCardGame.Data.Enums;

namespace TradingCardGame.Data.Models
{
    public class UserChannels
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string ChannelId { get; set; }

        public Channel Channel { get; set; }

        public ChannelUserRole Role { get; set; }
    }
}
