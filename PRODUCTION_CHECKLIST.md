# ✅ Production Deployment Kontrol Listesi

## 🔴 HTTP 500 Hatası Çözüm Adımları

### 1. appsettings.json Dosyasını Kontrol Edin

Hosting'de `appsettings.json` dosyasının mevcut olduğundan ve doğru olduğundan emin olun:

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

**ÖNEMLİ:** 
- `MultipleActiveResultSets=True` ekleyin
- Connection string'deki server adını kontrol edin (hosting sağlayıcınızdan alın)
- Database adını kontrol edin
- Kullanıcı adı ve şifreyi kontrol edin

---

### 2. Log Dosyalarını Kontrol Edin

#### Adım 1: Logs Klasörü Oluşturun

Hosting'de `logs` klasörünü oluşturun ve yazma izni verin.

#### Adım 2: Log Dosyalarını Okuyun

FTP/FileZilla ile `logs/stdout_*.log` dosyalarını indirin ve okuyun.

**Log dosyaları şu hataları gösterebilir:**
- Connection string hatası
- Veritabanı bulunamadı
- Kullanıcı adı/şifre hatası
- Migration hatası

---

### 3. Veritabanını Kontrol Edin

#### Adım 1: Veritabanının Oluşturulduğundan Emin Olun

Plesk panelinden veya SQL Server Management Studio ile:
1. Veritabanının mevcut olduğunu kontrol edin
2. `database-script.sql` dosyasını çalıştırın (eğer çalıştırılmadıysa)

#### Adım 2: Connection String'i Test Edin

SQL Server Management Studio ile connection string'i test edin:
- Server: `localhost\MSSQLSERVER2022` (veya hosting'in verdiği adres)
- Database: `dtomeral_randevu_sistemi`
- User: `dtomeral_randevu4`
- Password: `13579Mami.*`

---

### 4. Dosya İzinlerini Kontrol Edin

#### Gerekli İzinler:

1. **logs/** klasörü → Yazma izni
2. **wwwroot/** klasörü → Okuma izni
3. **Views/** klasörü → Okuma izni
4. **RandevuWeb.exe** → Çalıştırma izni

#### IIS Application Pool Identity:

Genellikle `IIS AppPool\YourAppPoolName` şeklindedir. Bu kullanıcıya yukarıdaki izinleri verin.

---

### 5. web.config Kontrolü

`web.config` dosyasında şu ayarlar olmalı:

```xml
<aspNetCore processPath=".\RandevuWeb.exe" 
            arguments="" 
            stdoutLogEnabled="true" 
            stdoutLogFile=".\logs\stdout" 
            stdoutLogMaxFiles="10"
            stdoutLogFileSizeLimitKB="1024"
            hostingModel="inprocess">
  <environmentVariables>
    <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
    <environmentVariable name="ASPNETCORE_DETAILEDERRORS" value="true" />
  </environmentVariables>
</aspNetCore>
```

---

### 6. Self-Contained Deployment Kontrolü

#### RandevuWeb.exe Mevcut mu?

Hosting'de `RandevuWeb.exe` dosyasının mevcut olduğundan emin olun.

Eğer yoksa, self-contained publish yapın:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
```

---

## 🔍 Yaygın Hatalar ve Çözümleri

### Hata 1: "No valid connection string found"
**Çözüm:** `appsettings.json` dosyasını oluşturun ve connection string ekleyin.

### Hata 2: "Cannot open database"
**Çözüm:** 
- Veritabanını oluşturun (`database-script.sql` çalıştırın)
- Connection string'deki database adını kontrol edin

### Hata 3: "Login failed for user"
**Çözüm:** 
- Connection string'deki kullanıcı adı ve şifreyi kontrol edin
- SQL Server'da kullanıcının mevcut olduğundan emin olun

### Hata 4: "Access Denied"
**Çözüm:** 
- `logs` klasörüne yazma izni verin
- IIS Application Pool identity'ye izin verin

### Hata 5: "RandevuWeb.exe not found"
**Çözüm:** 
- Self-contained publish yapın
- Tüm dosyaları hosting'e yükleyin

---

## 📋 Kontrol Listesi

### Dosyalar:
- [ ] `appsettings.json` mevcut ve doğru
- [ ] `web.config` mevcut
- [ ] `RandevuWeb.exe` mevcut
- [ ] `Migrations/` klasörü yüklendi
- [ ] `Views/` klasörü yüklendi
- [ ] `wwwroot/` klasörü yüklendi

### Ayarlar:
- [ ] Connection string doğru ve `MultipleActiveResultSets=True` var
- [ ] Veritabanı oluşturuldu
- [ ] `logs` klasörü oluşturuldu ve yazma izni verildi
- [ ] IIS Application Pool çalışıyor

### Test:
- [ ] `logs/stdout_*.log` dosyalarını kontrol ettiniz
- [ ] Connection string'i SQL Server Management Studio ile test ettiniz
- [ ] IIS Event Viewer'ı kontrol ettiniz

---

## 🆘 Hata Devam Ederse

1. **Log Dosyalarını İndirin**
   - `logs/stdout_*.log` dosyalarını FTP ile indirin
   - İçeriğini okuyun ve hata mesajını bulun

2. **IIS Event Viewer'ı Kontrol Edin**
   - Windows Event Viewer'da Application log'larını kontrol edin
   - Hata mesajlarını not edin

3. **Connection String'i Test Edin**
   - SQL Server Management Studio ile bağlantıyı test edin
   - Server adını, database adını, kullanıcı adını ve şifreyi doğrulayın

4. **Hosting Sağlayıcınızla İletişime Geçin**
   - SQL Server instance adını sorun
   - Connection string formatını sorun
   - Port numarasını sorun (varsa)

---

## 📝 Örnek Production appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\MSSQLSERVER2022;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Error"
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

## 🎯 Hızlı Çözüm

1. **appsettings.json Oluştur** → Yukarıdaki örneği kullan
2. **Logs Klasörü Oluştur** → Yazma izni ver
3. **Veritabanını Oluştur** → `database-script.sql` çalıştır
4. **Log Dosyalarını Kontrol Et** → `logs/stdout_*.log` oku
5. **IIS'i Yeniden Başlat** → Application Pool'u restart et

---

**ÖNEMLİ:** Log dosyaları en önemli bilgi kaynağıdır. Mutlaka kontrol edin!

