using LedAmbiental.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LedAmbiental.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly ILogger _logger;
        private readonly Contexto _contexto;

        public AccountController(ILogger<AccountController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Movimentacao");
            }
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
                    new Claim(ClaimTypes.Role, user.Funcao),
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

                    return RedirectToAction("Index", "Movimentacao");
                }

                ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
            }

            return View(model);
        }

        private async Task<ApplicationUser?> AuthenticateUser(string login, string senha)
        {
            var user = await _contexto.ApplicationUser
                .FirstOrDefaultAsync(u => u.Login == login && u.Senha == senha);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Usuário {Name} saiu da conta ás {Time}.",
                User.Identity.Name, DateTime.UtcNow);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}
