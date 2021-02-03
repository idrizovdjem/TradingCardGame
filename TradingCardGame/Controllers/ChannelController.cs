using Microsoft.AspNetCore.Mvc;

namespace TradingCardGame.Controllers
{
    public class ChannelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
