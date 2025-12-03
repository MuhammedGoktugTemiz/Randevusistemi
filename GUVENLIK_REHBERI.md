# 🔒 Güvenlik Rehberi

Bu dokümantasyon, randevu sisteminizin güvenlik özelliklerini ve şifre yönetimini açıklar.

## 🛡️ Güvenlik Özellikleri

### 1. Şifre Hash'leme
- Tüm şifreler **BCrypt** algoritması ile hash'lenir
- Şifreler asla düz metin olarak saklanmaz
- BCrypt, güçlü bir şifre hash'leme algoritmasıdır ve brute force saldırılarına karşı koruma sağlar

### 2. Rate Limiting (Brute Force Koruması)
- Her IP adresi için **5 başarısız giriş denemesi** sonrası **15 dakika** kilitlenir
- Bu özellik hem admin hem de doktor girişleri için geçerlidir
- Saldırganların şifreleri tahmin etmesini zorlaştırır

### 3. Cookie Güvenliği
- **HttpOnly**: JavaScript ile erişilemez (XSS koruması)
- **Secure**: HTTPS üzerinden gönderilir
- **SameSite**: CSRF saldırılarına karşı koruma

### 4. Dosya Güvenliği
- `Data/` klasörü ve `appsettings.json` dosyası `.gitignore`'a eklenmiştir
- Hassas bilgiler Git'e commit edilmez
- Production ortamında environment variables kullanılmalıdır

## 📝 Şifre Yönetimi

### Admin Şifresini Değiştirme

Admin şifresi `appsettings.json` dosyasında saklanır. Şifreyi değiştirmek için:

1. **Yeni şifreyi hash'leyin:**
   ```csharp
   // Program.cs veya bir console uygulamasında:
   var hasher = new BCryptPasswordHasher();
   var hashedPassword = hasher.HashPassword("yeni_sifreniz");
   Console.WriteLine(hashedPassword);
   ```

2. **appsettings.json'ı güncelleyin:**
   ```json
   {
     "DefaultUser": {
       "Username": "admin",
       "PasswordHash": "BURAYA_HASH_LENMIŞ_ŞİFRE_YAPIŞTIRIN"
     }
   }
   ```

### Doktor Şifrelerini Değiştirme

Doktor şifreleri `Data/doctors.json` dosyasında saklanır. Şifreleri değiştirmek için:

1. **Doktor düzenleme sayfasından şifreyi değiştirin** (şifre otomatik olarak hash'lenir)
2. **VEYA doğrudan JSON dosyasını düzenleyin** (hash'lenmiş şifre ile)

### Mevcut Şifreleri Hash'leme

Eğer sisteminizde düz metin şifreler varsa, bunları hash'lemek için:

1. **Bir migration scripti çalıştırın** (PasswordMigrationUtility kullanarak)
2. **VEYA doktor düzenleme sayfasından şifreleri tek tek güncelleyin**

## 🔐 Güvenlik Best Practices

### 1. Production Ortamı İçin
- `appsettings.json` dosyasını Git'e commit etmeyin
- Environment variables kullanın
- HTTPS kullanın (zaten aktif)
- Düzenli olarak şifreleri değiştirin

### 2. Şifre Seçimi
- En az 8 karakter kullanın
- Büyük harf, küçük harf, rakam ve özel karakter karışımı kullanın
- Yaygın şifrelerden kaçının (123456, password, vb.)

### 3. Erişim Kontrolü
- Sadece güvenilir kişilere admin erişimi verin
- Doktor hesaplarını düzenli olarak gözden geçirin
- Kullanılmayan hesapları silin

## 🚨 Güvenlik Uyarıları

### ⚠️ ÖNEMLİ:
1. **appsettings.json** dosyasını asla Git'e commit etmeyin
2. **Data/** klasörünü asla Git'e commit etmeyin
3. Production ortamında şifreleri environment variables olarak saklayın
4. Düzenli olarak güvenlik güncellemelerini kontrol edin

### 🔍 Güvenlik Kontrol Listesi
- [ ] Tüm şifreler hash'lenmiş durumda
- [ ] appsettings.json Git'e eklenmemiş
- [ ] Data/ klasörü Git'e eklenmemiş
- [ ] HTTPS aktif
- [ ] Rate limiting çalışıyor
- [ ] Cookie güvenlik ayarları aktif

## 📞 Destek

Güvenlik sorunları için:
1. Log dosyalarını kontrol edin
2. Rate limiting mesajlarını kontrol edin
3. Şifre hash'lerinin doğru formatda olduğundan emin olun

## 🔄 Şifre Hash Formatı

BCrypt hash'leri şu formatta olur:
```
$2a$12$xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

Eğer şifreniz bu formatta değilse, düz metin olarak saklanıyor demektir ve hash'lenmelidir.

