using System.Linq;
using TradingCardGame.Data;
using TradingCardGame.Models.Channel;

namespace TradingCardGame.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public ChannelInformationViewModel GetUsersChannel(string userId)
        {
            return this.context.Channels
                .Where(x => x.CreatorId == userId)
                .Select(x => new ChannelInformationViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    CurrentPlayers = this.context.UserChannels
                        .Count(ch => ch.ChannelId == x.Id),
                    MaxPlayers = x.MaxUsers
                })
                .FirstOrDefault();
        }
    }
}
