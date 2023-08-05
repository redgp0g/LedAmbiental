using LedAmbiental.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace LedAmbiental.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger _logger;


        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await AuthenticateUser(model.Login, model.Senha);

                if (user != null)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim("FullName", user.Nome),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
            }

            return View(model);
        }


        private async Task<ApplicationUser?> AuthenticateUser(string login, string senha)
        {
            if (login == "maria.rodriguez@contoso.com" && senha == "senha")
            {
                return new ApplicationUser
                {
                    Login = "maria.rodriguez@contoso.com",
                    Nome = "Maria Rodriguez"
                };
            }

            return null;
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Usuário {Name} saiu da conta ás {Time}.",
                User.Identity.Name, DateTime.UtcNow);

            #region snippet1
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            #endregion

            return RedirectToAction("Index", "Home");
        }
    }
}
