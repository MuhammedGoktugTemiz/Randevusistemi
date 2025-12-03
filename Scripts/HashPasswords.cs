// Bu dosyayı bir console uygulaması olarak çalıştırabilirsiniz
// veya Program.cs içinde bir kez çalıştırabilirsiniz

using RandevuWeb.Services;
using RandevuWeb.Utilities;

namespace RandevuWeb.Scripts;

/// <summary>
/// Mevcut şifreleri hash'lemek için script
/// Bu scripti bir kez çalıştırarak tüm şifreleri hash'leyebilirsiniz
/// </summary>
public class HashPasswordsScript
{
    public static void Run()
    {
        var hasher = new BCryptPasswordHasher();

        Console.WriteLine("=== Şifre Hash'leme Yardımcısı ===\n");

        // Admin şifresi için
        Console.WriteLine("Admin şifresi için hash oluştur:");
        Console.WriteLine("Örnek şifre: admin123");
        var adminHash = hasher.HashPassword("admin123");
        Console.WriteLine($"Hash: {adminHash}\n");

        // Doktor şifreleri için
        Console.WriteLine("Doktor şifreleri için hash'ler:");
        var doctorPasswords = new[] { "omer123", "soner123", "busra123" };
        foreach (var password in doctorPasswords)
        {
            var hash = hasher.HashPassword(password);
            Console.WriteLine($"Şifre: {password} -> Hash: {hash}");
        }

        Console.WriteLine("\n=== Hash'leme Tamamlandı ===");
        Console.WriteLine("Bu hash'leri appsettings.json ve doctors.json dosyalarına kopyalayın.");
    }
}

