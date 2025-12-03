using RandevuWeb.Services;

namespace RandevuWeb.Utilities;

/// <summary>
/// Şifreleri hash'lemek için yardımcı utility sınıfı
/// Bu sınıfı kullanarak mevcut düz metin şifreleri BCrypt hash'ine dönüştürebilirsiniz
/// </summary>
public static class PasswordHasherUtility
{
    /// <summary>
    /// Bir şifreyi hash'ler ve konsola yazdırır
    /// </summary>
    public static string HashPassword(string password)
    {
        var hasher = new BCryptPasswordHasher();
        return hasher.HashPassword(password);
    }

    /// <summary>
    /// Şifreyi hash'ler ve formatlanmış çıktı verir
    /// </summary>
    public static void PrintHashedPassword(string password)
    {
        var hashed = HashPassword(password);
        Console.WriteLine($"Şifre: {password}");
        Console.WriteLine($"Hash:  {hashed}");
        Console.WriteLine();
    }
}

