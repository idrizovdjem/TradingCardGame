using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingCardGame.Data.Models
{
    public class Post
    {
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Comments = new List<Comment>();
            this.Votes = new List<PostVote>();
        }

        public string Id { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        [Required]
        public string ChannelId { get; set; }

        public Channel Channel { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Content length should be at least 3 symbols.")]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<PostVote> Votes { get; set; }
    }
}
