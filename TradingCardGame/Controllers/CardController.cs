using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using TradingCardGame.Data.Enums;
using TradingCardGame.Data.Models;
using TradingCardGame.Models.Card;
using TradingCardGame.Services.Contracts;

namespace TradingCardGame.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CardController : Controller
    {
        private readonly ICardService cardService;
        private readonly IChannelService channelService;
        private readonly UserManager<ApplicationUser> userManager;

        public CardController(UserManager<ApplicationUser> userManager, ICardService cardService, IChannelService channelService)
        {
            this.cardService = cardService;
            this.userManager = userManager;
            this.channelService = channelService;
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

        public IActionResult Edit(string cardId)
        {
            var card = this.cardService.GetCardForEdit(cardId);
            return View(card);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCardInputModel input)
        {
            if(!ModelState.IsValid)
            {
                return View(input);
            }

            if (input.ChannelName == string.Empty)
            {
                ModelState.AddModelError("ChannelName", "Channel is invalid or can't create cards");
                return View(input);
            }

            await this.cardService.UpdateAsync(input);

            return RedirectToAction("Index", new { input.ChannelName });
        }

        public IActionResult GetCardsWithStatus(string channelName, CardStatus status)
        {
            if(status == 0)
            {
                return BadRequest();
            }

            var channelId = this.channelService.GetChannelIdByName(channelName);
            var cards = this.cardService.GetCardsWithStatus(channelId, status);
            
            return Json(cards);
        }

        public IActionResult Review(string cardId)
        {
            var card = this.cardService.GetCardForReview(cardId);
            return View(card);
        }

        public async Task<IActionResult> SendReview(string cardId, string decision, string channelName)
        {
            CardStatus status = decision switch
            {
                "approve" => CardStatus.Approved,
                "archive" => CardStatus.Archived,
                _ => CardStatus.Deleted
            };

            await this.cardService.ReviewCard(cardId, status);
            
            return RedirectToAction("Index", new { channelName });
        }
    }
}
