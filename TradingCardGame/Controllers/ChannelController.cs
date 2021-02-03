using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TradingCardGame.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ChannelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
