namespace TradingCardGame.Models.Channel
{
    public class ChannelInformationViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int CurrentPlayers { get; set; }

        public int MaxPlayers { get; set; }

        public int PostsCount { get; set; }

        public int TotalLikes { get; set; }

        public int CardsCount { get; set; }
    }
}
