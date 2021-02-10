using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingCardGame.Services;
using TradingCardGame.Data.Enums;
using TradingCardGame.Data.Models;
using Microsoft.AspNetCore.Identity;
using TradingCardGame.Models.Channel;

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
            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            var channel = new ChannelIndexViewModel()
            {
                Channels = this.channelService.GetUserChannels(user.Id),
                IsChannelCreated = this.channelService.HaveUserCreatedChannel(user.Id)
            };
            return View(channel);
        }

        public IActionResult GetChannelContent(string channelName)
        {
            var userId = this.userManager.GetUserAsync(User).GetAwaiter().GetResult().Id;
            var channelContent = this.channelService.GetChannelContent(channelName, userId);
            return Json(channelContent);
        }

        public async Task<IActionResult> CreateChannel()
        {
            var user = await this.userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChannel(CreateChannelInputModel input)
        {
            var user = await this.userManager.GetUserAsync(User);
            if (user == null)
            {
                return Redirect("/Account/Login");
            }

            if(!ModelState.IsValid)
            {
                return View(input);
            }

            if(!this.channelService.IsChannelNameAvailable(input.Name))
            {
                ModelState.AddModelError("Name", "Channel name is already taken.");
                return View(input);
            }

            var channel = new Channel()
            {
                CreatorId = user.Id,
                Name = input.Name,
                IsDeleted = false,
                MaxUsers = input.MaxPlayers,
                Security = input.Security
            };

            await this.channelService.CreateAsync(channel);
            await this.channelService.AddUserToChannel(user.Id, channel.Name, ChannelUserRole.Administrator);

            return RedirectToAction("Index");
        }
    }
}
