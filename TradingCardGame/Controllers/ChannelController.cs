using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using TradingCardGame.Data.Models;
using TradingCardGame.Models.Channel;
using TradingCardGame.Services.Contracts;

namespace TradingCardGame.Controllers
{
    [Authorize]
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

        public IActionResult CreateChannel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChannel(CreateChannelInputModel input)
        {
            var user = await this.userManager.GetUserAsync(User);

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
            await this.channelService.AddUserToChannelAsync(user.Id, channel.Id);

            return RedirectToAction("Index");
        }
    }
}
