using System.Collections.Generic;

namespace TradingCardGame.Models.Channel
{
    public class ChannelIndexViewModel
    {
        public List<string> Channels { get; set; }

        public ChannelViewModel SelectedChannel { get; set; }
    }
}
