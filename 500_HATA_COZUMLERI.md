# 🔧 HTTP Error 500.0 - ASP.NET Core IIS Hosting Failure Çözümleri

## ❌ Hata: HTTP Error 500.0 - ASP.NET Core IIS hosting failure (in-process)

Bu hata, uygulamanın başlatılamadığı anlamına gelir. En yaygın nedenler:

---

## 🔍 1. appsettings.json Eksik veya Yanlış

### Sorun:
`appsettings.json` dosyası eksik veya connection string yanlış.

### Çözüm:

1. **appsettings.json Dosyasını Oluşturun**

Hosting'de `appsettings.json` dosyasını oluşturun:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\MSSQLSERVER2022;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "DefaultUser": {
    "Username": "admin",
    "Password": "Admin123.*"
  },
  "WhatsApp": {
    "AccessToken": "YOUR_WHATSAPP_ACCESS_TOKEN",
    "PhoneNumberId": "YOUR_PHONE_NUMBER_ID"
  }
}
```

2. **Connection String'i Kontrol Edin**

- Server adını kontrol edin (`localhost\\MSSQLSERVER2022` veya hosting'in verdiği adres)
- Database adını kontrol edin
- Kullanıcı adı ve şifreyi kontrol edin
- `TrustServerCertificate=True` ekleyin

---

## 🔍 2. Veritabanı Bağlantı Hatası

### Sorun:
Veritabanına bağlanılamıyor veya veritabanı yok.

### Çözüm:

1. **Veritabanını Oluşturun**

Plesk panelinden veya SQL Server Management Studio ile `database-script.sql` dosyasını çalıştırın.

2. **Connection String'i Test Edin**

SQL Server Management Studio ile bağlantıyı test edin.

3. **Log Dosyalarını Kontrol Edin**

`logs/stdout_*.log` dosyalarını kontrol ederek hatayı görebilirsiniz.

---

## 🔍 3. Log Dosyalarını Aktifleştirin

### Adım 1: Logs Klasörü Oluşturun

Hosting'de `logs` klasörünü oluşturun ve yazma izni verin.

### Adım 2: web.config Kontrolü

`web.config` dosyasında şu ayarlar olmalı:

```xml
<aspNetCore processPath=".\RandevuWeb.exe" 
            arguments="" 
            stdoutLogEnabled="true" 
            stdoutLogFile=".\logs\stdout" 
            stdoutLogMaxFiles="10"
            stdoutLogFileSizeLimitKB="1024"
            hostingModel="inprocess">
```

### Adım 3: Log Dosyalarını Okuyun

`logs/stdout_*.log` dosyalarını okuyarak hatayı görebilirsiniz.

---

## 🔍 4. Dosya İzinleri Sorunu

### Sorun:
IIS Application Pool identity'nin dosyalara yazma izni yok.

### Çözüm:

1. **Application Pool Identity'yi Bulun**

IIS Manager'da Application Pool'un identity'sini kontrol edin (genellikle `IIS AppPool\YourAppPoolName`).

2. **İzinleri Verin**

Şu klasörlere yazma izni verin:
- `logs/` klasörü
- `wwwroot/` klasörü (eğer dosya yükleme varsa)

---

## 🔍 5. RandevuWeb.exe Bulunamıyor

### Sorun:
Self-contained deployment yapılmamış veya dosyalar eksik.

### Çözüm:

1. **Self-Contained Publish Yapın**

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
```

2. **Tüm Dosyaları Yükleyin**

`publish/` klasöründeki **TÜM** dosyaları hosting'e yükleyin.

3. **web.config Kontrolü**

`web.config` dosyasında `processPath=".\RandevuWeb.exe"` olmalı.

---

## 🔍 6. Migration Sorunu

### Sorun:
EF Core migration'ları uygulanmamış.

### Çözüm:

1. **Veritabanını Oluşturun**

`database-script.sql` dosyasını çalıştırın.

2. **Migration Klasörünü Kontrol Edin**

`Migrations/` klasörünün yüklendiğinden emin olun.

3. **Uygulama İlk Açılışta Otomatik Migration Yapacak**

Eğer hata alırsanız, `logs/stdout_*.log` dosyalarını kontrol edin.

---

## 📋 Hızlı Kontrol Listesi

### Dosyalar:
- [ ] `appsettings.json` dosyası mevcut ve doğru
- [ ] `web.config` dosyası mevcut
- [ ] `RandevuWeb.exe` dosyası mevcut
- [ ] `Migrations/` klasörü yüklendi
- [ ] `Views/` klasörü yüklendi
- [ ] `wwwroot/` klasörü yüklendi

### Ayarlar:
- [ ] Connection string doğru
- [ ] Veritabanı oluşturuldu
- [ ] `logs` klasörü oluşturuldu ve yazma izni verildi
- [ ] IIS Application Pool çalışıyor

### Test:
- [ ] `logs/stdout_*.log` dosyalarını kontrol edin
- [ ] IIS Event Viewer'ı kontrol edin
- [ ] Connection string'i SQL Server Management Studio ile test edin

---

## 🆘 Yaygın Hata Mesajları ve Çözümleri

### "No valid connection string found"
**Çözüm:** `appsettings.json` dosyasını oluşturun ve connection string ekleyin.

### "Cannot open database"
**Çözüm:** Veritabanını oluşturun (`database-script.sql` çalıştırın).

### "Login failed for user"
**Çözüm:** Connection string'deki kullanıcı adı ve şifreyi kontrol edin.

### "Access Denied"
**Çözüm:** `logs` klasörüne yazma izni verin.

### "RandevuWeb.exe not found"
**Çözüm:** Self-contained publish yapın ve tüm dosyaları yükleyin.

---

## 📞 Detaylı Hata Ayıklama

### 1. Stdout Log'larını Aktifleştirin

`web.config` dosyasında:
```xml
stdoutLogEnabled="true"
stdoutLogFile=".\logs\stdout"
```

### 2. Log Dosyalarını Okuyun

`logs/stdout_*.log` dosyalarını okuyarak hatayı görebilirsiniz.

### 3. IIS Event Viewer'ı Kontrol Edin

Windows Event Viewer'da Application log'larını kontrol edin.

### 4. Connection String'i Test Edin

SQL Server Management Studio ile connection string'i test edin.

---

## ✅ Başarılı Deployment Kontrolü

Uygulama başarıyla çalışıyorsa:

1. ✅ Ana sayfa açılıyor
2. ✅ Login sayfası açılıyor
3. ✅ Veritabanı bağlantısı çalışıyor
4. ✅ Log dosyaları oluşturuluyor

---

## 🎯 Hızlı Çözüm Adımları

1. **appsettings.json Oluştur**
   - Production connection string ile

2. **Veritabanını Oluştur**
   - `database-script.sql` çalıştır

3. **Logs Klasörü Oluştur**
   - Yazma izni ver

4. **Log Dosyalarını Kontrol Et**
   - `logs/stdout_*.log` dosyalarını oku

5. **IIS'i Yeniden Başlat**
   - Application Pool'u restart et

---

## 📝 Örnek appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\MSSQLSERVER2022;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "DefaultUser": {
    "Username": "admin",
    "Password": "Admin123.*"
  },
  "WhatsApp": {
    "AccessToken": "YOUR_WHATSAPP_ACCESS_TOKEN",
    "PhoneNumberId": "YOUR_PHONE_NUMBER_ID"
  }
}
```

---

**Sorun devam ederse:** `logs/stdout_*.log` dosyalarını kontrol edin ve hata mesajını paylaşın.

