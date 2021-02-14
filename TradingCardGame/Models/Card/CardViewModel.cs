namespace TradingCardGame.Models.Card
{
    public class CardViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }
    }
}