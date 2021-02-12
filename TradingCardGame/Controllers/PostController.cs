using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using TradingCardGame.Services;
using TradingCardGame.Data.Models;
using TradingCardGame.Models.Posts;

namespace TradingCardGame.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class PostController : Controller
    {
        private readonly IPostService postService;
        private readonly IChannelService channelService;
        private readonly UserManager<ApplicationUser> userManager;

        public PostController(IPostService postService, IChannelService channelService, UserManager<ApplicationUser> userManager)
        {
            this.postService = postService;
            this.channelService = channelService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostInputModel input)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var channelId = this.channelService.GetChannelIdByName(input.ChannelName);
            var userId = this.userManager.GetUserAsync(User).GetAwaiter().GetResult().Id;
            var post = await this.postService.CreateAsync(channelId, userId, input.Content);

            return Json(post);
        }

        public async Task<IActionResult> Vote(string postId)
        {
            var userId = this.userManager.GetUserAsync(User).GetAwaiter().GetResult().Id;
            await this.postService.Vote(postId, userId);
            return Ok();
        }
    }
}
