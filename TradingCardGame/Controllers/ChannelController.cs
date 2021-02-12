using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingCardGame.Services;
using TradingCardGame.Data.Enums;
using TradingCardGame.Data.Models;
using Microsoft.AspNetCore.Identity;
using TradingCardGame.Models.Channel;
using Microsoft.AspNetCore.Authorization;
using System;

namespace TradingCardGame.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ChannelController : Controller
    {
        private readonly IUserService userService;
        private readonly IChannelService channelService;
        private readonly UserManager<ApplicationUser> userManager;

        public ChannelController(IUserService userService, IChannelService channelService, UserManager<ApplicationUser> userManager)
        {
            this.channelService = channelService;
            this.userManager = userManager;
            this.userService = userService;
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

        [HttpPost]
        public async Task<IActionResult> Manage(CreateChannelInputModel input)
        {
            if(!ModelState.IsValid)
            {
                return View(input);
            }

            var user = await this.userManager.GetUserAsync(User);

            var userChannel = this.userService.GetUsersChannel(user.Id);
            if(userChannel.Name != input.Name)
            {
                if(!this.channelService.IsChannelNameAvailable(input.Name))
                {
                    ModelState.AddModelError("Name", "Channel name is taken");
                    return View(input);
                }
            }

            if(input.MaxPlayers < userChannel.CurrentPlayers)
            {
                ModelState.AddModelError("MaxPlayers", "Can't set max players below current players count");
                return View(input);
            }

            await this.channelService.UpdateChannelAsync(input, user.Id);

            return RedirectToAction("Index");
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
            ChannelUserRole channelRole;
            var isRoleParsed = Enum.TryParse(role, out channelRole);
            if(!isRoleParsed)
            {
                return BadRequest();
            }

            var channelOnwer = await this.userManager.GetUserAsync(User);

            await this.channelService.AddUserToRoleAsync(channelOnwer.Id, userId, channelRole);

            return Ok();
        }
    }
}
