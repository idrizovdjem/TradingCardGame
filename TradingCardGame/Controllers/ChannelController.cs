using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingCardGame.Services;
using TradingCardGame.Data.Enums;
using TradingCardGame.Data.Models;
using Microsoft.AspNetCore.Identity;
using TradingCardGame.Models.Channel;
using Microsoft.AspNetCore.Authorization;

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
            await this.channelService.AddUserToChannelAsync(user.Id, channel.Id, ChannelUserRole.Administrator);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Leave(string channelName)
        {
            var user = await this.userManager.GetUserAsync(User);

            if(channelName == "Global Channel")
            {
                return RedirectToAction("Index");
            }

            await this.channelService.RemoveUserFromChannelAsync(channelName, user.Id);

            return RedirectToAction("Index");
        }

        public IActionResult Information(string channelName)
        {
            var channelInformation = this.channelService.GetChannelInformation(channelName);
            return View(channelInformation);
        }

        public async Task<IActionResult> Manage(string channelName)
        {
            var user = await this.userManager.GetUserAsync(User);

            if(!this.channelService.IsUserOwner(channelName, user.Id))
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
