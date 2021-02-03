using System.Linq;
using TradingCardGame.Data;
using System.Threading.Tasks;
using TradingCardGame.Data.Models;

namespace TradingCardGame.Services
{
    public class ChannelService : IChannelService
    {
        private readonly ApplicationDbContext context;

        public ChannelService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(string channelName, string userId)
        {
            var channel = new Channel()
            {
                Name = channelName,
                CreatorId = userId,
                IsDeleted = false
            };

            await this.context.Channels.AddAsync(channel);
            await this.context.SaveChangesAsync();
        }

        public bool Exists(string channelName)
        {
            return this.context.Channels.Any(channel => channel.Name == channelName);
        }
    }
}
