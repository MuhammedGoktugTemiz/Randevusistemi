using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandevuWeb.Services;
using RandevuWeb.Utilities;

namespace RandevuWeb.Controllers;

/// <summary>
/// Güvenlik ve şifre yönetimi için controller
/// Sadece admin kullanıcıları erişebilir
/// </summary>
[Authorize]
public class SecurityController : Controller
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDataService _dataService;
    private readonly IWebHostEnvironment _env;

    public SecurityController(IPasswordHasher passwordHasher, IDataService dataService, IWebHostEnvironment env)
    {
        _passwordHasher = passwordHasher;
        _dataService = dataService;
        _env = env;
    }

    /// <summary>
    /// Mevcut düz metin şifreleri hash'ler
    /// Sadece development ortamında çalışır
    /// </summary>
    [HttpPost]
    public IActionResult MigratePasswords()
    {
        // Sadece development ortamında çalışsın
        if (!_env.IsDevelopment())
        {
            return Forbid();
        }

        try
        {
            var dataPath = Path.Combine(_env.ContentRootPath, "Data");
            var migrationUtility = new PasswordMigrationUtility(_passwordHasher, dataPath);
            migrationUtility.MigrateDoctorPasswords();

            return Json(new { success = true, message = "Şifreler başarıyla hash'lendi!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Hata: {ex.Message}" });
        }
    }

    /// <summary>
    /// Bir şifreyi hash'ler (test için)
    /// </summary>
    [HttpPost]
    public IActionResult HashPassword([FromBody] HashPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Şifre boş olamaz" });
        }

        var hashed = _passwordHasher.HashPassword(request.Password);
        return Json(new { password = request.Password, hash = hashed });
    }
}

public class HashPasswordRequest
{
    public string Password { get; set; } = string.Empty;
}

