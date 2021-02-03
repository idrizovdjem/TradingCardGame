using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingCardGame.Data.Models
{
    public class Channel
    {
        public Channel()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Users = new List<UserChannels>();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<UserChannels> Users { get; set; }
    }
}
