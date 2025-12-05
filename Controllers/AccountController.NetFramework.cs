using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Configuration;
using RandevuWeb.Models;
using RandevuWeb.Services;

namespace RandevuWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPasswordHasher _passwordHasher;
        private const int MaxLoginAttempts = 5;
        private const int LockoutDurationMinutes = 15;

        public AccountController()
        {
            _passwordHasher = DependencyResolver.Current.GetService<IPasswordHasher>();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            // Rate limiting kontrolü (Session kullanarak)
            var clientIp = Request.UserHostAddress ?? "unknown";
            var sessionKey = $"login_attempts_{clientIp}";
            
            var attemptCount = Session[sessionKey] as int? ?? 0;
            if (attemptCount >= MaxLoginAttempts)
            {
                ModelState.AddModelError("", $"Çok fazla başarısız giriş denemesi! Lütfen {LockoutDurationMinutes} dakika sonra tekrar deneyin.");
                return View(model);
            }

            var username = WebConfigurationManager.AppSettings["DefaultUser:Username"];
            var storedPasswordHash = WebConfigurationManager.AppSettings["DefaultUser:PasswordHash"] ?? 
                                   WebConfigurationManager.AppSettings["DefaultUser:Password"];

            // Kullanıcı adı kontrolü
            if (string.IsNullOrWhiteSpace(model.Username) || !model.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
            {
                IncrementLoginAttempts(sessionKey);
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                return View(model);
            }

            // Şifre doğrulama
            bool passwordValid = false;
            if (!string.IsNullOrEmpty(storedPasswordHash) && 
                (storedPasswordHash.StartsWith("$2a$") || storedPasswordHash.StartsWith("$2b$") || storedPasswordHash.StartsWith("$2y$")))
            {
                // BCrypt hash formatı
                passwordValid = _passwordHasher.VerifyPassword(model.Password ?? "", storedPasswordHash);
            }
            else
            {
                // Eski düz metin şifre (geriye dönük uyumluluk)
                passwordValid = model.Password == storedPasswordHash;
            }

            if (!passwordValid)
            {
                IncrementLoginAttempts(sessionKey);
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                return View(model);
            }

            // Başarılı giriş - rate limiting'i temizle
            Session.Remove(sessionKey);

            // Forms Authentication ile giriş yap
            FormsAuthentication.SetAuthCookie(model.Username, true);

            return RedirectToAction("Index", "Home");
        }

        private void IncrementLoginAttempts(string sessionKey)
        {
            var attemptCount = Session[sessionKey] as int? ?? 0;
            Session[sessionKey] = attemptCount + 1;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("LoginSelection", "Home");
        }
    }
}

