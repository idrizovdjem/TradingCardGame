﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TradingCardGame.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Cards = new List<UserCards>();
            this.Channels = new List<UserChannels>();
            this.PostVotes = new List<PostVote>();
        }

        public ICollection<UserCards> Cards { get; set; }

        public ICollection<UserChannels> Channels { get; set; }

        public ICollection<PostVote> PostVotes { get; set; }
    }
}
