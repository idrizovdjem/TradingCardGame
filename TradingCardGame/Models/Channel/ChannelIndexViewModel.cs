using System.Collections.Generic;

namespace TradingCardGame.Models.Channel
{
    public class ChannelIndexViewModel
    {
        public IEnumerable<string> Channels { get; set; }

        public bool IsChannelCreated { get; set; }
    }
}
