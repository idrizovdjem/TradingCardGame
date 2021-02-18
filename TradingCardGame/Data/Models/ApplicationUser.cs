using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace TradingCardGame.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.DeckCards = new List<UserCards>();
            this.Channels = new List<UserChannels>();
            this.PostVotes = new List<PostVote>();
            this.CreatedCards = new List<Card>();
            this.Decks = new List<Deck>();
        }

        public ICollection<UserCards> DeckCards { get; set; }

        public ICollection<UserChannels> Channels { get; set; }

        public ICollection<PostVote> PostVotes { get; set; }

        public ICollection<Card> CreatedCards { get; set; }

        public ICollection<Deck> Decks { get; set; }
    }
}
