# 🚀 VPS/Dedicated Server'da ASP.NET Core Deployment Rehberi

## 📋 Gereksinimler

- Windows Server (2016, 2019, 2022)
- Administrator erişimi
- En az 2GB RAM
- En az 20GB disk alanı
- Statik IP adresi

---

## ✅ ADIM 1: Windows Server Hazırlığı

### 1.1. Windows Server'a Bağlanın

**Remote Desktop ile:**
1. Windows'ta **Remote Desktop Connection** açın
2. Server IP adresini girin
3. Administrator kullanıcı adı ve şifresi ile giriş yapın

**Veya:**
- Plesk panel üzerinden RDP erişimi
- VNC bağlantısı
- SSH (Linux için)

---

## ✅ ADIM 2: IIS Kurulumu

### 2.1. IIS'i Kontrol Edin

1. **Server Manager** açın
2. **Manage** → **Add Roles and Features** tıklayın
3. **Role-based or feature-based installation** seçin
4. **Web Server (IIS)** seçin
5. **Next** → **Install**

**Veya PowerShell ile:**
```powershell
Install-WindowsFeature -name Web-Server -IncludeManagementTools
```

### 2.2. Gerekli IIS Özelliklerini Ekleyin

**PowerShell ile:**
```powershell
# ASP.NET Core için gerekli özellikler
Install-WindowsFeature -name Web-ASP-NET45
Install-WindowsFeature -name Web-ISAPI-Ext
Install-WindowsFeature -name Web-ISAPI-Filter
Install-WindowsFeature -name Web-Metabase
Install-WindowsFeature -name Web-Net-Ext45
Install-WindowsFeature -name Web-Request-Monitor
Install-WindowsFeature -name Web-Static-Content
Install-WindowsFeature -name Web-Windows-Auth
```

---

## ✅ ADIM 3: ASP.NET Core Hosting Bundle Kurulumu

### 3.1. Hosting Bundle'ı İndirin

1. Tarayıcıda şu adrese gidin:
   ```
   https://dotnet.microsoft.com/download/dotnet/8.0
   ```

2. **ASP.NET Core Runtime 8.0.x - Windows Hosting Bundle** indirin
   - Dosya adı: `dotnet-hosting-8.0.x-win.exe`

### 3.2. Hosting Bundle'ı Kurun

1. İndirilen `.exe` dosyasını çalıştırın
2. **Install** butonuna tıklayın
3. Kurulum tamamlanana kadar bekleyin
4. **Close** butonuna tıklayın

**Veya PowerShell ile (otomatik):**
```powershell
# Hosting Bundle'ı indir ve kur
$url = "https://download.visualstudio.microsoft.com/download/pr/12345678-1234-1234-1234-123456789012/abc123def456/dotnet-hosting-8.0.x-win.exe"
$output = "$env:TEMP\dotnet-hosting.exe"
Invoke-WebRequest -Uri $url -OutFile $output
Start-Process -FilePath $output -ArgumentList "/quiet" -Wait
```

### 3.3. IIS'i Yeniden Başlatın

**PowerShell ile:**
```powershell
iisreset
```

**Veya:**
1. **Server Manager** → **Tools** → **Internet Information Services (IIS) Manager**
2. Sağ tıklayın → **Restart**

---

## ✅ ADIM 4: SQL Server Kurulumu

### 4.1. SQL Server Express Kurulumu (Eğer yoksa)

1. [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads) indirin
2. Kurulum sihirbazını çalıştırın
3. **Basic** kurulum seçin
4. Kurulum tamamlanana kadar bekleyin

**Not:** Eğer SQL Server zaten kuruluysa bu adımı atlayın.

### 4.2. SQL Server Authentication'ı Aktifleştirin

1. **SQL Server Management Studio (SSMS)** açın
2. Server'a bağlanın
3. Sağ tıklayın → **Properties**
4. **Security** sekmesine gidin
5. **SQL Server and Windows Authentication mode** seçin
6. **OK** tıklayın
7. SQL Server servisini yeniden başlatın

**PowerShell ile:**
```powershell
# SQL Server servisini yeniden başlat
Restart-Service MSSQLSERVER
```

### 4.3. Veritabanını Oluşturun

1. **SQL Server Management Studio** açın
2. Server'a bağlanın
3. **New Query** tıklayın
4. `database-script.sql` dosyasını açın
5. İçeriğini kopyalayıp query penceresine yapıştırın
6. **Execute** (F5) tıklayın

---

## ✅ ADIM 5: Uygulama Dosyalarını Hazırlama

### 5.1. Local'de Publish Yapın

**PowerShell'de:**
```powershell
# Proje klasörüne gidin
cd "C:\Users\Muhammed Göktuğ\Desktop\randevu web"

# Self-contained publish yapın
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
```

### 5.2. Dosyaları ZIP'e Paketleyin

1. `publish` klasörüne gidin
2. Tüm dosyaları seçin
3. Sağ tıklayın → **Send to** → **Compressed (zipped) folder**
4. ZIP dosyasını oluşturun

---

## ✅ ADIM 6: Dosyaları Server'a Yükleme

### 6.1. FTP ile Yükleme

**FileZilla kullanarak:**
1. FileZilla'yı açın
2. **File** → **Site Manager**
3. Yeni site ekleyin:
   - **Host:** Server IP adresi
   - **Protocol:** FTP
   - **Port:** 21
   - **User:** FTP kullanıcı adı
   - **Password:** FTP şifresi
4. **Connect** tıklayın
5. ZIP dosyasını `C:\inetpub\wwwroot\randevu` klasörüne yükleyin

### 6.2. Remote Desktop ile Yükleme

1. **Remote Desktop** ile server'a bağlanın
2. ZIP dosyasını server'a kopyalayın (copy-paste veya network share)
3. ZIP dosyasını `C:\inetpub\wwwroot\randevu` klasörüne çıkarın

### 6.3. Dosyaları Çıkarın

1. ZIP dosyasına sağ tıklayın
2. **Extract All...** seçin
3. `C:\inetpub\wwwroot\randevu` klasörüne çıkarın

---

## ✅ ADIM 7: IIS'te Website Oluşturma

### 7.1. IIS Manager'ı Açın

1. **Server Manager** → **Tools** → **Internet Information Services (IIS) Manager**

### 7.2. Website Oluşturun

1. **Sites** → sağ tıklayın → **Add Website**
2. Şu bilgileri girin:
   - **Site name:** randevu
   - **Application pool:** randevu (yeni oluşturulacak)
   - **Physical path:** `C:\inetpub\wwwroot\randevu`
   - **Binding:**
     - **Type:** http veya https
     - **IP address:** All Unassigned veya belirli IP
     - **Port:** 80 (http) veya 443 (https)
     - **Host name:** randevu.dtomeralbayrak.com (opsiyonel)
3. **OK** tıklayın

### 7.3. Application Pool Ayarlarını Yapın

1. **Application Pools** → **randevu** → çift tıklayın
2. Şu ayarları yapın:
   - **.NET CLR Version:** No Managed Code
   - **Managed Pipeline Mode:** Integrated
   - **Start Mode:** AlwaysRunning
3. **Advanced Settings:**
   - **Idle Time-out:** 0 (devre dışı)
   - **Start Mode:** AlwaysRunning
4. **OK** tıklayın

---

## ✅ ADIM 8: appsettings.json Oluşturma

### 8.1. appsettings.json Dosyasını Oluşturun

1. `C:\inetpub\wwwroot\randevu` klasörüne gidin
2. Yeni dosya oluşturun: `appsettings.json`
3. İçeriğini şu şekilde doldurun:

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

**Not:** Connection string'i kendi SQL Server bilgilerinize göre güncelleyin.

---

## ✅ ADIM 9: Logs Klasörü ve İzinler

### 9.1. Logs Klasörü Oluşturun

**PowerShell ile:**
```powershell
New-Item -Path "C:\inetpub\wwwroot\randevu\logs" -ItemType Directory
```

### 9.2. İzinleri Ayarlayın

**PowerShell ile:**
```powershell
# Application Pool identity'ye yazma izni ver
$acl = Get-Acl "C:\inetpub\wwwroot\randevu\logs"
$permission = "IIS AppPool\randevu","FullControl","ContainerInherit,ObjectInherit","None","Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule $permission
$acl.SetAccessRule($accessRule)
Set-Acl "C:\inetpub\wwwroot\randevu\logs" $acl
```

---

## ✅ ADIM 10: Firewall Ayarları

### 10.1. Port 80 ve 443'ü Açın

**PowerShell ile:**
```powershell
# HTTP (Port 80)
New-NetFirewallRule -DisplayName "HTTP" -Direction Inbound -LocalPort 80 -Protocol TCP -Action Allow

# HTTPS (Port 443)
New-NetFirewallRule -DisplayName "HTTPS" -Direction Inbound -LocalPort 443 -Protocol TCP -Action Allow
```

**Veya Windows Firewall GUI ile:**
1. **Windows Firewall** açın
2. **Advanced Settings**
3. **Inbound Rules** → **New Rule**
4. **Port** seçin → **Next**
5. **TCP** → **Specific local ports:** 80,443 → **Next**
6. **Allow the connection** → **Next**
7. Tüm profilleri seçin → **Next**
8. **Name:** HTTP/HTTPS → **Finish**

---

## ✅ ADIM 11: SSL Sertifikası (HTTPS için)

### 11.1. Let's Encrypt ile Ücretsiz SSL (Opsiyonel)

**Win-ACME kullanarak:**
1. [Win-ACME](https://www.win-acme.com/) indirin
2. Çalıştırın ve sihirbazı takip edin
3. Domain'inizi seçin
4. Sertifikayı otomatik olarak yükler

**Veya:**
- Hosting sağlayıcınızdan SSL sertifikası alın
- IIS'te SSL sertifikası yükleyin

---

## ✅ ADIM 12: Test ve Kontrol

### 12.1. Uygulamayı Test Edin

1. Tarayıcıda `http://SERVER_IP` veya `http://randevu.dtomeralbayrak.com` adresine gidin
2. Uygulamanın açıldığını kontrol edin

### 12.2. Log Dosyalarını Kontrol Edin

1. `C:\inetpub\wwwroot\randevu\logs\stdout_*.log` dosyalarını kontrol edin
2. Hata varsa log dosyalarını okuyun

### 12.3. IIS Log'larını Kontrol Edin

1. `C:\inetpub\logs\LogFiles\W3SVC*` klasörüne gidin
2. En son log dosyasını açın
3. Hataları kontrol edin

---

## 🆘 Sorun Giderme

### Sorun 1: "500.21 - Bad Module"

**Çözüm:**
- ASP.NET Core Hosting Bundle'ın yüklü olduğundan emin olun
- IIS'i yeniden başlatın: `iisreset`

### Sorun 2: "Access Denied"

**Çözüm:**
- `logs` klasörüne yazma izni verin
- Application Pool identity'ye izin verin

### Sorun 3: "Connection String Error"

**Çözüm:**
- `appsettings.json` dosyasını kontrol edin
- SQL Server'ın çalıştığından emin olun
- Connection string'i test edin

### Sorun 4: "Port Already in Use"

**Çözüm:**
- Başka bir port kullanın (örn: 8080)
- Veya mevcut website'i durdurun

---

## 📋 Kontrol Listesi

- [ ] Windows Server'a bağlandınız
- [ ] IIS kurulu
- [ ] ASP.NET Core Hosting Bundle kurulu
- [ ] SQL Server kurulu ve çalışıyor
- [ ] Veritabanı oluşturuldu
- [ ] Uygulama dosyaları server'a yüklendi
- [ ] IIS'te website oluşturuldu
- [ ] Application Pool ayarları yapıldı
- [ ] `appsettings.json` oluşturuldu
- [ ] `logs` klasörü oluşturuldu ve izinler verildi
- [ ] Firewall portları açıldı
- [ ] SSL sertifikası yüklendi (HTTPS için)
- [ ] Uygulama test edildi

---

## 🎯 Hızlı Komutlar

### IIS'i Yeniden Başlat
```powershell
iisreset
```

### Application Pool'u Yeniden Başlat
```powershell
Restart-WebAppPool -Name "randevu"
```

### Website'i Başlat/Durdur
```powershell
# Başlat
Start-Website -Name "randevu"

# Durdur
Stop-Website -Name "randevu"
```

### Log Dosyalarını Temizle
```powershell
Remove-Item "C:\inetpub\wwwroot\randevu\logs\*" -Force
```

---

## 📞 Yardım

Sorun yaşarsanız:
1. Log dosyalarını kontrol edin
2. IIS Event Viewer'ı kontrol edin
3. Windows Event Viewer'ı kontrol edin
4. `NET_FRAMEWORK_4.8_SORUNU.md` dosyasına bakın

---

**Başarılar!** 🎉

