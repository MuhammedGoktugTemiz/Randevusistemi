using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RandevuWeb.Models;
using RandevuWeb.Services;
using System.Security.Claims;

namespace RandevuWeb.Controllers;

public class AccountController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMemoryCache _cache;
    private const int MaxLoginAttempts = 5;
    private const int LockoutDurationMinutes = 15;

    public AccountController(IConfiguration configuration, IPasswordHasher passwordHasher, IMemoryCache cache)
    {
        _configuration = configuration;
        _passwordHasher = passwordHasher;
        _cache = cache;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(User model)
    {
        // Rate limiting kontrolü
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var cacheKey = $"login_attempts_{clientIp}";
        
        if (_cache.TryGetValue(cacheKey, out int attemptCount) && attemptCount >= MaxLoginAttempts)
        {
            ModelState.AddModelError("", $"Çok fazla başarısız giriş denemesi! Lütfen {LockoutDurationMinutes} dakika sonra tekrar deneyin.");
            return View(model);
        }

        var defaultUser = _configuration.GetSection("DefaultUser");
        var username = defaultUser["Username"];
        var storedPasswordHash = defaultUser["PasswordHash"] ?? defaultUser["Password"]; // Eski şifreler için geriye dönük uyumluluk

        // Kullanıcı adı kontrolü
        if (string.IsNullOrWhiteSpace(model.Username) || !model.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
        {
            IncrementLoginAttempts(cacheKey);
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            return View(model);
        }

        // Şifre doğrulama - hash'lenmiş veya düz metin (geriye dönük uyumluluk)
        bool passwordValid = false;
        if (storedPasswordHash.StartsWith("$2a$") || storedPasswordHash.StartsWith("$2b$") || storedPasswordHash.StartsWith("$2y$"))
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
            IncrementLoginAttempts(cacheKey);
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            return View(model);
        }

        // Başarılı giriş - rate limiting'i temizle
        _cache.Remove(cacheKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Username)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "Home");
    }

    private void IncrementLoginAttempts(string cacheKey)
    {
        if (_cache.TryGetValue(cacheKey, out int attemptCount))
        {
            _cache.Set(cacheKey, attemptCount + 1, TimeSpan.FromMinutes(LockoutDurationMinutes));
        }
        else
        {
            _cache.Set(cacheKey, 1, TimeSpan.FromMinutes(LockoutDurationMinutes));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}

