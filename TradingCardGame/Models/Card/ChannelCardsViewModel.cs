using System.Collections.Generic;

namespace TradingCardGame.Models.Card
{
    public class ChannelCardsViewModel
    {
        public string UserRole { get; set; }

        public IEnumerable<CardViewModel> ApprovedCards { get; set; }
    }
}
