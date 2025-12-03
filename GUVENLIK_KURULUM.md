# 🔒 Güvenlik Kurulum Rehberi

Bu rehber, sisteminizin güvenliğini artırmak için yapmanız gereken adımları açıklar.

## ✅ Yapılan Güvenlik İyileştirmeleri

1. ✅ **BCrypt Şifre Hash'leme** - Tüm şifreler artık güvenli bir şekilde hash'leniyor
2. ✅ **Rate Limiting** - Brute force saldırılarına karşı koruma (5 deneme sonrası 15 dakika kilit)
3. ✅ **Cookie Güvenliği** - HttpOnly, Secure, SameSite ayarları aktif
4. ✅ **Git Güvenliği** - Hassas dosyalar .gitignore'a eklendi

## 📋 Kurulum Adımları

### 1. Paketleri Yükleme

Proje klasöründe şu komutu çalıştırın:

```bash
dotnet restore
```

Bu komut BCrypt.Net-Next paketini yükleyecektir.

### 2. Admin Şifresini Hash'leme

Mevcut admin şifrenizi (`admin123`) hash'lemek için:

**Seçenek A: Program.cs içinde geçici kod ekleyin:**

```csharp
// Program.cs'in sonuna, app.Run()'dan önce ekleyin:
if (args.Length > 0 && args[0] == "hash-admin")
{
    var hasher = new BCryptPasswordHasher();
    var hash = hasher.HashPassword("admin123"); // Şifrenizi buraya yazın
    Console.WriteLine($"Admin şifre hash'i: {hash}");
    return;
}
```

Sonra şu komutu çalıştırın:
```bash
dotnet run -- hash-admin
```

**Seçenek B: SecurityController'ı kullanın:**

1. Uygulamayı çalıştırın
2. Admin olarak giriş yapın
3. `/Security/HashPassword` endpoint'ine POST isteği gönderin:
   ```json
   {
     "password": "admin123"
   }
   ```

### 3. appsettings.json'ı Güncelleme

`appsettings.json` dosyasını açın ve şu şekilde güncelleyin:

```json
{
  "DefaultUser": {
    "Username": "admin",
    "PasswordHash": "BURAYA_YUKARIDA_OLUŞTURDUĞUNUZ_HASH_YAPIŞTIRIN"
  }
}
```

**ÖNEMLİ:** `Password` yerine `PasswordHash` kullanın. Sistem otomatik olarak eski `Password` alanını da destekler (geriye dönük uyumluluk).

### 4. Doktor Şifrelerini Hash'leme

Doktor şifreleri otomatik olarak hash'lenecektir. Ancak mevcut şifreleri hemen hash'lemek için:

**Seçenek A: Doktor Düzenleme Sayfasından:**

1. Her doktor için düzenleme sayfasına gidin
2. "Yeni Şifre" alanına mevcut şifreyi girin
3. Kaydedin (şifre otomatik hash'lenecek)

**Seçenek B: SecurityController Migration Endpoint'i:**

Development ortamında `/Security/MigratePasswords` endpoint'ine POST isteği gönderin.

### 5. Git Güvenliği Kontrolü

`.gitignore` dosyasının şu satırları içerdiğinden emin olun:

```
Data/
Data/**/*.json
appsettings.json
```

Eğer `appsettings.json` veya `Data/` klasörü daha önce Git'e eklenmişse:

```bash
git rm --cached appsettings.json
git rm -r --cached Data/
git commit -m "Hassas dosyaları Git'ten kaldır"
```

## 🔍 Güvenlik Kontrol Listesi

Kurulumdan sonra şunları kontrol edin:

- [ ] `dotnet restore` başarıyla tamamlandı
- [ ] Admin şifresi hash'lenmiş durumda (`appsettings.json`'da `PasswordHash` var)
- [ ] Doktor şifreleri hash'lenmiş durumda (`doctors.json`'da şifreler `$2a$` ile başlıyor)
- [ ] `appsettings.json` Git'e commit edilmemiş
- [ ] `Data/` klasörü Git'e commit edilmemiş
- [ ] Uygulama çalışıyor ve giriş yapılabiliyor

## 🧪 Test Etme

### Admin Girişi Testi:

1. `/Account/Login` sayfasına gidin
2. Kullanıcı adı: `admin`
3. Şifre: `admin123` (veya yeni belirlediğiniz şifre)
4. Giriş yapabildiğinizi kontrol edin

### Rate Limiting Testi:

1. Yanlış şifre ile 5 kez giriş yapmayı deneyin
2. 6. denemede "Çok fazla başarısız giriş denemesi" mesajını görmelisiniz
3. 15 dakika bekleyin veya farklı bir IP'den deneyin

### Doktor Girişi Testi:

1. `/DoctorAuth/Login` sayfasına gidin
2. Doktor kullanıcı adı ve şifresi ile giriş yapın
3. Giriş yapabildiğinizi kontrol edin

## 🚨 Önemli Notlar

1. **Production Ortamı:** Production'da `appsettings.json` yerine **Environment Variables** kullanın
2. **Şifre Değiştirme:** Admin şifresini değiştirmek için yeni hash oluşturup `appsettings.json`'a ekleyin
3. **Yedekleme:** `Data/` klasörünü düzenli olarak yedekleyin (Git'e değil, güvenli bir yere)
4. **HTTPS:** Production'da mutlaka HTTPS kullanın

## 📞 Sorun Giderme

### "BCrypt.Net-Next bulunamadı" Hatası:

```bash
dotnet restore
dotnet build
```

### "Şifre hash'i doğrulanamıyor" Hatası:

- Hash'in doğru kopyalandığından emin olun
- Hash'in `$2a$`, `$2b$` veya `$2y$` ile başladığından emin olun
- Şifrenin doğru girildiğinden emin olun

### Rate Limiting Çalışmıyor:

- Memory cache'in çalıştığından emin olun (`Program.cs`'de `AddMemoryCache()` var mı?)
- IP adresinin doğru algılandığını kontrol edin

## 📚 Ek Kaynaklar

- [Güvenlik Rehberi](./GUVENLIK_REHBERI.md) - Detaylı güvenlik bilgileri
- [BCrypt Dokümantasyonu](https://github.com/BcryptNet/bcrypt.net)

