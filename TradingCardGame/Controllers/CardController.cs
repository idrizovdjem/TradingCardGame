using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using TradingCardGame.Services;
using TradingCardGame.Data.Models;

namespace TradingCardGame.Controllers
{
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
    }
}
