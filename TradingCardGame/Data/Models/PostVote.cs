using System.ComponentModel.DataAnnotations;

namespace TradingCardGame.Data.Models
{
    public class PostVote
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string PostId { get; set; }

        public Post Post { get; set; }

        public bool IsDeleted { get; set; }
    }
}
