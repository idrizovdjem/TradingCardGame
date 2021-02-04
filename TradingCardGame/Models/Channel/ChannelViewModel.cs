using System.Collections.Generic;
using TradingCardGame.Models.Posts;

namespace TradingCardGame.Models.Channel
{
    public class ChannelViewModel
    {
        public string Name { get; set; }

        public ICollection<PostViewModel> Posts { get; set; }
    }
}
