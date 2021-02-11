using TradingCardGame.Models.Channel;

namespace TradingCardGame.Services
{
    public interface IUserService
    {
        ChannelInformationViewModel GetUsersChannel(string userId);
    }
}
