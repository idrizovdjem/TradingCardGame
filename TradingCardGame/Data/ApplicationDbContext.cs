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

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<PostVote> PostVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Comment>(entity =>
            {
                entity
                    .HasOne(c => c.Post)
                    .WithMany(p => p.Comments)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Post>(entity =>
            {
                entity
                    .HasOne(p => p.Channel)
                    .WithMany(c => c.Posts)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<PostVote>(entity =>
            {
                entity
                    .HasOne(pv => pv.Post)
                    .WithMany(p => p.Votes)
                    .OnDelete(DeleteBehavior.NoAction);

                entity
                    .HasOne(pv => pv.User)
                    .WithMany(u => u.PostVotes)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            base.OnModelCreating(builder);
        }
    }
}
