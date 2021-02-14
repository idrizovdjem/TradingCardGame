using TradingCardGame.Models.Channel;

namespace TradingCardGame.Services.Contracts
{
    public interface IUserService
    {
        ChannelInformationViewModel GetUsersChannel(string userId);
    }
}
