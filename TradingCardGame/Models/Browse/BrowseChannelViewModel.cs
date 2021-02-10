using TradingCardGame.Models.Enums;

namespace TradingCardGame.Models.Browse
{
    public class BrowseChannelViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int CurrentPlayers { get; set; }

        public int MaxPlayers { get; set; }

        public ChannelStatus Status { get; set; }
    }
}
