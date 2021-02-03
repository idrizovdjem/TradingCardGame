using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingCardGame.Services;
using TradingCardGame.Data.Enums;
using TradingCardGame.Data.Models;
using Microsoft.AspNetCore.Identity;
using TradingCardGame.Models.Account;

namespace TradingCardGame.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private readonly IChannelService channelService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IChannelService channelService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.channelService = channelService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var user = new ApplicationUser()
            {
                Email = input.Email,
                UserName = input.Email
            };

            var registrationResult = await this.userManager.CreateAsync(user, input.Password);
            if (!registrationResult.Succeeded)
            {
                foreach (var error in registrationResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View(input);
            }

            await this.channelService.AddUserToChannel(user.Id, "Global Channel", ChannelUserRole.User);
            await this.signInManager.SignInAsync(user, true);


            return Redirect("/Channel/Index");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginInputModel input)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            var loginResult = await this.signInManager.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, false);
            if(!loginResult.Succeeded)
            {
                ModelState.AddModelError("Invalid login attempt", "Invalid email or password");
                return View();
            }

            return Redirect("/Channel/Index");
        }
    }
}
