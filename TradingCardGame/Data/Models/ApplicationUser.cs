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
        }

        public ICollection<UserCards> Cards { get; set; }
    }
}
