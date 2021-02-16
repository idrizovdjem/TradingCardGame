using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using TradingCardGame.Data.Models;
using TradingCardGame.Services.Contracts;
using TradingCardGame.Models.Card;

namespace TradingCardGame.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CardController : Controller
    {
        private readonly ICardService cardService;
        private readonly UserManager<ApplicationUser> userManager;

        public CardController(UserManager<ApplicationUser> userManager, ICardService cardService)
        {
            this.cardService = cardService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string channelName)
        {
            var user = await this.userManager.GetUserAsync(User);
            var channelCardsModel = this.cardService.GetChannelCards(channelName, user.Id);
            return View(channelCardsModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCardInputModel input)
        {
            if(!ModelState.IsValid)
            {
                return View(input);
            }

            var user = await this.userManager.GetUserAsync(User);
            if(input.ChannelName == string.Empty)
            {
                ModelState.AddModelError("ChannelName", "Channel is invalid or can't create cards");
                return View(input);
            }

            await this.cardService.CreateAsync(input, user.Id);

            return RedirectToAction("Index", new { channelName = input.ChannelName } );
        }
    }
}
