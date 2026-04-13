using System.Text;
using Microsoft.AspNetCore.Mvc;
using SuperHeroesDB.Data;
using SuperHeroesDB.Models;
using System.Security.Cryptography;

namespace SuperHeroesDB.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HeroesContext _context;

        public AuthenticationController(HeroesContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                bool loginExists = _context.User.Any(u => u.Login.ToLower() == user.Login.ToLower());
                bool emailExists = _context.User.Any(u => u.Email.ToLower() == user.Email.ToLower());

                if (loginExists)
                {
                    ModelState.AddModelError("Login", "This login is taken. Please choose another one.");
                }

                if (emailExists)
                {
                    ModelState.AddModelError("Email", "This email address is already registered.");
                }

                if (!ModelState.IsValid)
                {
                    return View(user);
                }

                user.HashPassword = HashString(user.HashPassword);

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Login == login);

            if (user != null && user.HashPassword == HashString(password))
            {
                Response.Cookies.Append("UserLogin", user.Login, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Wrong login or password.");
            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserLogin");
            return RedirectToAction("Index", "Home");
        }

        private string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
