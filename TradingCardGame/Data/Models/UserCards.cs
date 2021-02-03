namespace TradingCardGame.Data.Models
{
    public class UserCards
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string CardId { get; set; }

        public Card Card { get; set; }
    }
}
