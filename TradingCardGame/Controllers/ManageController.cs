using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using TradingCardGame.Data.Enums;
using TradingCardGame.Data.Models;
using TradingCardGame.Models.Channel;
using TradingCardGame.Services.Contracts;

namespace TradingCardGame.Controllers
{
    public class ManageController : Controller
    {
        private readonly IUserService userService;
        private readonly IChannelService channelService;
        private readonly UserManager<ApplicationUser> userManager;

        public ManageController(IUserService userService, IChannelService channelService, UserManager<ApplicationUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.channelService = channelService;
        }

        public async Task<IActionResult> Leave(string channelName)
        {
            var user = await this.userManager.GetUserAsync(User);

            if (channelName == "Global Channel")
            {
                return Redirect("/Channel/Index");
            }

            await this.channelService.RemoveUserFromChannelAsync(channelName, user.Id);

            return Redirect("/Channel/Index");
        }

        public IActionResult Information(string channelName)
        {
            var channelInformation = this.channelService.GetChannelInformation(channelName);
            return View(channelInformation);
        }

        public async Task<IActionResult> Index(string channelName)
        {
            var user = await this.userManager.GetUserAsync(User);

            if (!this.channelService.IsUserOwner(channelName, user.Id))
            {
                return Redirect("/Channel/Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateChannelInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var user = await this.userManager.GetUserAsync(User);

            var userChannel = this.userService.GetUsersChannel(user.Id);
            if (userChannel.Name != input.Name)
            {
                if (!this.channelService.IsChannelNameAvailable(input.Name))
                {
                    ModelState.AddModelError("Name", "Channel name is taken");
                    return View(input);
                }
            }

            if (input.MaxPlayers < userChannel.CurrentPlayers)
            {
                ModelState.AddModelError("MaxPlayers", "Can't set max players below current players count");
                return View(input);
            }

            await this.channelService.UpdateChannelAsync(input, user.Id);

            return Redirect("/Channel/Index");
        }

        public async Task<IActionResult> GetUsers()
        {
            var user = await this.userManager.GetUserAsync(User);
            var channelUsers = this.channelService.GetChannelUsers(user.Id);
            return Json(channelUsers);
        }

        public async Task<IActionResult> RemoveUser(string userId)
        {
            var channelOwner = await this.userManager.GetUserAsync(User);
            var channelName = this.channelService.GetChannelName(channelOwner.Id);

            await this.channelService.RemoveUserFromChannelAsync(channelName, userId);

            return Ok();
        }

        public async Task<IActionResult> ChangeRole(string userId, string role)
        {
            var isRoleParsed = Enum.TryParse(role, out ChannelUserRole channelRole);
            if (!isRoleParsed)
            {
                return BadRequest();
            }

            var channelOnwer = await this.userManager.GetUserAsync(User);

            await this.channelService.AddUserToRoleAsync(channelOnwer.Id, userId, channelRole);

            return Ok();
        }
    }
}
