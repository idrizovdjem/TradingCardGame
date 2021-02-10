using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingCardGame.Services;
using TradingCardGame.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace TradingCardGame.Controllers
{
    public class BrowseController : Controller
    {
        private readonly IChannelService channelService;
        private readonly UserManager<ApplicationUser> userManager;

        public BrowseController(IChannelService channelService, UserManager<ApplicationUser> userManager)
        {
            this.channelService = channelService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            if(User == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = this.userManager.GetUserAsync(User).GetAwaiter().GetResult().Id;
            var topTenChannels = this.channelService.GetTopTenChannels(userId);
            return View(topTenChannels);
        }

        public async Task<IActionResult> JoinChannel(string channelId)
        {
            if (User == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = this.userManager.GetUserAsync(User).GetAwaiter().GetResult().Id;
            await this.channelService.AddUserToChannelAsync(userId, channelId, Data.Enums.ChannelUserRole.User);
            return Redirect("Index");
        }

        public IActionResult GetChannelsContainingName(string name)
        {
            var userId = this.userManager.GetUserAsync(User).GetAwaiter().GetResult().Id;
            var channels = this.channelService.GetChannelsContainingName(name, userId);
            return Json(channels);
        }
    }
}
