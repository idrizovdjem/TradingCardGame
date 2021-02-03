using System.Threading.Tasks;

namespace TradingCardGame.Services
{
    public interface IChannelService
    {
        bool Exists(string channelName);

        Task CreateAsync(string channelName, string userId);
    }
}
