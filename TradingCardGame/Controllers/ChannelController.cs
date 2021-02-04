using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingCardGame.Services;
using TradingCardGame.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace TradingCardGame.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class ChannelController : Controller
    {
        private readonly IChannelService channelService;
        private readonly UserManager<ApplicationUser> userManager;

        public ChannelController(IChannelService channelService, UserManager<ApplicationUser> userManager)
        {
            this.channelService = channelService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(User);
            if(user == null)
            {
                return Redirect("/Account/Login");
            }

            var channel = this.channelService.GetUserChannels(user.Id);
            return View(channel);
        }

        public IActionResult GetChannelContent(string channelName)
        {
            var channelContent = this.channelService.GetChannelContent(channelName);
            return Json(channelContent);
        }
    }
}
