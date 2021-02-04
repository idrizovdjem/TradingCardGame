using System;
using System.ComponentModel.DataAnnotations;

namespace TradingCardGame.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        [Required]
        public string PostId { get; set; }

        public Post Post { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Comment length should be at least 3 symbols.")]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Score { get; set; }

        public bool IsDeleted { get; set; }
    }
}
