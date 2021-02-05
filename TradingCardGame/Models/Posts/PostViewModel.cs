namespace TradingCardGame.Models.Posts
{
    public class PostViewModel
    {
        public string Id { get; set; }

        public string Creator { get; set; }

        public string Content { get; set; }

        public string CreatedOn { get; set; }

        public int Score { get; set; }

        public bool IsVoted { get; set; }
    }
}
