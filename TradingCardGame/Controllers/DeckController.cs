using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using TradingCardGame.Data.Models;
using TradingCardGame.Services.Contracts;
using TradingCardGame.Models.Deck;

namespace TradingCardGame.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class DeckController : Controller
    {
        private readonly IDeckService deckService;
        private readonly UserManager<ApplicationUser> userManager;

        public DeckController(UserManager<ApplicationUser> userManager, IDeckService deckService)
        {
            this.deckService = deckService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(string channelName)
        {
            var user = await this.userManager.GetUserAsync(User);
            var decks = this.deckService.GetUserDecks(user.Id, channelName);
            return View(decks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDeckInputModel input)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index", new { input.ChannelName });
            }

            var user = await this.userManager.GetUserAsync(User);
            await this.deckService.CreateAsync(input, user.Id);

            return RedirectToAction("Index", new { input.ChannelName });
        }
    }
}
