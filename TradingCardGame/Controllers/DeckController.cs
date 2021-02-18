using Microsoft.AspNetCore.Mvc;

namespace TradingCardGame.Controllers
{
    public class DeckController : Controller
    {
        public IActionResult Index(string channelName)
        {
            return View();
        }
    }
}
