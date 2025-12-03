using RandevuWeb.Models;
using RandevuWeb.Services;
using System.Text.Json;

namespace RandevuWeb.Utilities;

/// <summary>
/// Mevcut düz metin şifreleri BCrypt hash'ine dönüştürmek için utility
/// Bu dosyayı bir kez çalıştırarak tüm şifreleri hash'leyebilirsiniz
/// </summary>
public class PasswordMigrationUtility
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly string _dataPath;

    public PasswordMigrationUtility(IPasswordHasher passwordHasher, string dataPath)
    {
        _passwordHasher = passwordHasher;
        _dataPath = dataPath;
    }

    /// <summary>
    /// Doctors.json dosyasındaki tüm şifreleri hash'ler
    /// </summary>
    public void MigrateDoctorPasswords()
    {
        var filePath = Path.Combine(_dataPath, "doctors.json");
        if (!File.Exists(filePath))
        {
            Console.WriteLine("doctors.json dosyası bulunamadı!");
            return;
        }

        var json = File.ReadAllText(filePath);
        var doctors = JsonSerializer.Deserialize<List<Doctor>>(json) ?? new List<Doctor>();

        bool hasChanges = false;
        foreach (var doctor in doctors)
        {
            // Eğer şifre zaten hash'lenmişse atla
            if (string.IsNullOrWhiteSpace(doctor.Password) || 
                doctor.Password.StartsWith("$2a$") || 
                doctor.Password.StartsWith("$2b$") || 
                doctor.Password.StartsWith("$2y$"))
            {
                continue;
            }

            // Şifreyi hash'le
            doctor.Password = _passwordHasher.HashPassword(doctor.Password);
            hasChanges = true;
            Console.WriteLine($"✓ {doctor.Name} şifresi hash'lendi");
        }

        if (hasChanges)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var updatedJson = JsonSerializer.Serialize(doctors, options);
            File.WriteAllText(filePath, updatedJson);
            Console.WriteLine("\n✓ Tüm doktor şifreleri başarıyla hash'lendi!");
        }
        else
        {
            Console.WriteLine("\n✓ Tüm şifreler zaten hash'lenmiş durumda.");
        }
    }
}

