using TradingCardGame.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TradingCardGame.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }

        public DbSet<UserCards> UserCards { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<UserChannels> UserChannels { get; set; }
    }
}
