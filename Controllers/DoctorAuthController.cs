using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RandevuWeb.Models;
using RandevuWeb.Services;
using System.Security.Claims;

namespace RandevuWeb.Controllers;

public class DoctorAuthController : Controller
{
    private readonly IDataService _dataService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMemoryCache _cache;
    private const int MaxLoginAttempts = 5;
    private const int LockoutDurationMinutes = 15;

    public DoctorAuthController(IDataService dataService, IPasswordHasher passwordHasher, IMemoryCache cache)
    {
        _dataService = dataService;
        _passwordHasher = passwordHasher;
        _cache = cache;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true && User.HasClaim("DoctorId", ""))
        {
            var doctorIdClaim = User.FindFirst("DoctorId")?.Value;
            if (!string.IsNullOrEmpty(doctorIdClaim) && int.TryParse(doctorIdClaim, out int doctorId))
            {
                return RedirectToAction("MyAppointments", "DoctorAuth", new { id = doctorId });
            }
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(DoctorLoginModel model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
        {
            ModelState.AddModelError("", "Kullanıcı adı ve şifre gereklidir!");
            return View(model ?? new DoctorLoginModel());
        }

        var username = model.Username?.Trim();
        var password = model.Password?.Trim();

        if (string.IsNullOrWhiteSpace(username))
        {
            ModelState.AddModelError("", "Kullanıcı adı gereklidir!");
            return View(model);
        }

        // Rate limiting kontrolü
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var cacheKey = $"doctor_login_attempts_{clientIp}_{username}";
        
        if (_cache.TryGetValue(cacheKey, out int attemptCount) && attemptCount >= MaxLoginAttempts)
        {
            ModelState.AddModelError("", $"Çok fazla başarısız giriş denemesi! Lütfen {LockoutDurationMinutes} dakika sonra tekrar deneyin.");
            return View(model);
        }

        var doctor = _dataService.GetDoctorByUsername(username);
        
        if (doctor == null)
        {
            IncrementLoginAttempts(cacheKey);
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            return View(model);
        }

        // Şifre doğrulama - hash'lenmiş veya düz metin (geriye dönük uyumluluk)
        var doctorPassword = doctor.Password?.Trim() ?? string.Empty;
        bool passwordValid = false;
        
        if (string.IsNullOrWhiteSpace(doctorPassword))
        {
            IncrementLoginAttempts(cacheKey);
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            return View(model);
        }

        if (doctorPassword.StartsWith("$2a$") || doctorPassword.StartsWith("$2b$") || doctorPassword.StartsWith("$2y$"))
        {
            // BCrypt hash formatı
            passwordValid = _passwordHasher.VerifyPassword(password ?? "", doctorPassword);
        }
        else
        {
            // Eski düz metin şifre (geriye dönük uyumluluk)
            passwordValid = doctorPassword.Equals(password, StringComparison.Ordinal);
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
            new Claim(ClaimTypes.Name, doctor.Name),
            new Claim("DoctorId", doctor.Id.ToString()),
            new Claim("DoctorUsername", doctor.Username)
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

        return RedirectToAction("MyAppointments", "DoctorAuth", new { id = doctor.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "DoctorAuth");
    }

    [HttpGet]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult MyAppointments(int id, string? date = null, string? view = "week")
    {
        var doctor = _dataService.GetDoctor(id);
        if (doctor == null)
        {
            return NotFound();
        }

        // Check if logged in doctor matches
        var doctorIdClaim = User.FindFirst("DoctorId")?.Value;
        if (string.IsNullOrEmpty(doctorIdClaim) || doctorIdClaim != id.ToString())
        {
            // If no doctor claim, redirect to login
            if (string.IsNullOrEmpty(doctorIdClaim))
            {
                return RedirectToAction("Login", "DoctorAuth");
            }
            return Forbid();
        }

        var appointments = _dataService.GetAppointments()
            .Where(a => a.DoctorId == id)
            .OrderBy(a => a.Start)
            .ToList();

        var now = DateTime.Now;
        var today = now.Date;
        
        // Parse date parameter or use today
        DateTime selectedDate = today;
        if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var parsedDate))
        {
            selectedDate = parsedDate.Date;
        }
        
        // Calculate week/month based on view type
        DateTime startOfWeek, endOfWeek, startOfMonth, endOfMonth;
        
        if (view == "month")
        {
            startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            endOfMonth = startOfMonth.AddMonths(1);
            startOfWeek = startOfMonth;
            endOfWeek = endOfMonth;
        }
        else
        {
            startOfWeek = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
            if (selectedDate.DayOfWeek == DayOfWeek.Sunday) startOfWeek = selectedDate.AddDays(-6);
            endOfWeek = startOfWeek.AddDays(7);
            startOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            endOfMonth = startOfMonth.AddMonths(1);
        }

        ViewBag.Doctor = doctor;
        ViewBag.Appointments = appointments;
        ViewBag.Today = today;
        ViewBag.SelectedDate = selectedDate;
        ViewBag.StartOfWeek = startOfWeek;
        ViewBag.EndOfWeek = endOfWeek;
        ViewBag.StartOfMonth = startOfMonth;
        ViewBag.EndOfMonth = endOfMonth;
        ViewBag.Now = now;
        ViewBag.View = view ?? "week";

        return View();
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
}

public class DoctorLoginModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

