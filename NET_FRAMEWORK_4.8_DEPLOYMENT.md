# .NET Framework 4.8 Deployment Rehberi

## 🚨 ÖNEMLİ: .NET Framework 4.8 Deployment Farklıdır!

.NET Framework 4.8 projeleri ASP.NET Core gibi `dotnet publish` komutu ile publish edilmez. Visual Studio'da **Build** yapıp dosyaları manuel yüklemeniz gerekir.

---

## 📋 ADIM ADIM DEPLOYMENT

### 1️⃣ Visual Studio'da Build Et

1. Visual Studio'da projeyi açın
2. **Solution Configuration** → **Release** seçin
3. **Build** → **Rebuild Solution** yapın
4. Build başarılı olmalı (hata varsa düzeltin)

### 2️⃣ Yüklenecek Dosyaları Hazırla

#### ✅ MUTLAKA YÜKLENMESİ GEREKEN DOSYALAR:

```
📁 Bin/Release/ klasöründeki TÜM dosyalar:
   ✅ RandevuWeb.dll
   ✅ RandevuWeb.pdb (opsiyonel, debug için)
   ✅ Tüm dependency DLL'leri (EntityFramework.dll, BCrypt.Net-Next.dll, vb.)
   
📁 Proje kök dizini:
   ✅ web.config (connection string hosting'e göre güncellenmiş)
   ✅ Global.asax
   ✅ Global.asax.cs (bin'de compiled olacak)
   
📁 Views/ klasörü (TAMAMEN):
   ✅ Views/Account/
   ✅ Views/Appointment/
   ✅ Views/Calendar/
   ✅ Views/Doctor/
   ✅ Views/DoctorAuth/
   ✅ Views/Home/
   ✅ Views/Notifications/
   ✅ Views/Patient/
   ✅ Views/Reports/
   ✅ Views/Settings/
   ✅ Views/Shared/
   ✅ Views/_ViewImports.cshtml
   ✅ Views/_ViewStart.cshtml
   ✅ Views/web.config
   
📁 wwwroot/ klasörü (TAMAMEN):
   ✅ wwwroot/css/
   ✅ wwwroot/js/
   
📁 App_Start/ klasörü (bin'de compiled olacak ama source da olabilir):
   ✅ App_Start/BundleConfig.cs
   ✅ App_Start/FilterConfig.cs
   ✅ App_Start/RouteConfig.cs
   ✅ App_Start/UnityConfig.cs
   
📁 packages/ klasörü (NuGet paketleri):
   ✅ packages/ klasörünün TAMAMI
   VEYA
   ✅ Bin/Release/ klasöründeki tüm DLL'ler (paketler zaten bin'de olacak)
```

### 3️⃣ web.config'i Hosting'e Göre Güncelle

Hosting'deki SQL Server bilgilerini alıp `web.config` dosyasındaki connection string'i güncelleyin:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=SQL_SERVER_ADRESI;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**ÖNEMLİ:** `Server=` kısmına hosting'inizin SQL Server adresini yazın (örn: `sqlXXX.guzel.net.tr` veya `localhost\MSSQLSERVER2022`)

### 4️⃣ Dosyaları Hosting'e Yükle

#### Yöntem A: FTP ile (FileZilla, WinSCP, vb.)

1. FTP bilgilerinizi alın (Plesk panelinden)
2. Tüm dosyaları yükleyin:
   - `bin/Release/` klasöründeki TÜM dosyalar → hosting'deki `bin/` klasörüne
   - `Views/` klasörü → hosting'deki `Views/` klasörüne
   - `wwwroot/` klasörü → hosting'deki `wwwroot/` klasörüne
   - `web.config` → hosting'in kök dizinine
   - `Global.asax` → hosting'in kök dizinine
   - `App_Start/` klasörü → hosting'e (opsiyonel, bin'de compiled olacak)

#### Yöntem B: Plesk File Manager ile

1. Plesk panelinde **File Manager**'ı açın
2. Dosyaları tek tek veya klasör olarak yükleyin

### 5️⃣ IIS Application Pool Ayarları

Plesk panelinde veya IIS Manager'da:

1. **Application Pool** seçin
2. **.NET CLR Version** → **v4.0** seçin (veya **No Managed Code** - .NET Framework 4.8 için)
3. **Managed Pipeline Mode** → **Integrated** seçin
4. **Start Mode** → **AlwaysRunning** (opsiyonel)

### 6️⃣ Veritabanını Oluştur

1. Plesk panelinden **SQL Server** → **Databases** bölümüne gidin
2. `database-script.sql` dosyasını çalıştırın
3. Veya SQL Server Management Studio ile bağlanıp script'i çalıştırın

### 7️⃣ Test Et

1. Tarayıcıda `randevu.dtomeralbayrak.com` adresini açın
2. Giriş sayfası görünmeli
3. Hata varsa IIS log dosyalarını kontrol edin

---

## 🔍 SORUN GİDERME

### HTTP 500 Hatası

#### 1. Connection String Kontrolü
`web.config` dosyasındaki connection string'i kontrol edin:
- SQL Server adresi doğru mu?
- Database adı doğru mu?
- Kullanıcı adı ve şifre doğru mu?

#### 2. DLL Dosyaları Kontrolü
`bin/` klasöründe şu DLL'ler olmalı:
- `RandevuWeb.dll`
- `EntityFramework.dll`
- `EntityFramework.SqlServer.dll`
- `BCrypt.Net-Next.dll`
- `System.Web.Mvc.dll`
- `Unity.Container.dll`
- `Unity.Mvc5.dll`
- `Newtonsoft.Json.dll`
- Ve diğer dependency'ler

#### 3. IIS Log Dosyalarını Kontrol Et
Plesk panelinde veya hosting'de:
- `logs/` klasöründeki error log'ları okuyun
- Hangi DLL eksik veya hata veriyor görebilirsiniz

#### 4. Application Pool Kontrolü
- Application Pool çalışıyor mu?
- .NET Framework 4.8 yüklü mü? (hosting'de genelde yüklüdür)
- Application Pool'un doğru klasöre işaret ettiğinden emin olun

#### 5. web.config Syntax Kontrolü
`web.config` dosyasında XML syntax hatası olmamalı. Kontrol edin:
- Tüm tag'ler kapatılmış mı?
- Özel karakterler escape edilmiş mi? (`&` → `&amp;`)

### DLL Eksik Hatası

Eğer "Could not load file or assembly" hatası alıyorsanız:

1. `bin/Release/` klasöründeki TÜM DLL'leri kontrol edin
2. Eksik DLL'leri `packages/` klasöründen kopyalayın
3. Veya Visual Studio'da **Copy Local = True** yapıp tekrar build edin

### Veritabanı Bağlantı Hatası

1. Connection string'i kontrol edin
2. SQL Server'ın çalıştığından emin olun
3. Firewall ayarlarını kontrol edin
4. Kullanıcı adı ve şifrenin doğru olduğundan emin olun

---

## 📦 HIZLI DEPLOYMENT SCRIPT

PowerShell script'i ile otomatik deployment:

```powershell
# Build et
msbuild "randevu web.sln" /p:Configuration=Release /p:Platform="Any CPU"

# Deployment klasörü oluştur
$deployPath = ".\deploy"
if (Test-Path $deployPath) { Remove-Item $deployPath -Recurse -Force }
New-Item -ItemType Directory -Path $deployPath

# Dosyaları kopyala
Copy-Item "bin\Release\*" -Destination "$deployPath\bin\" -Recurse -Force
Copy-Item "Views" -Destination "$deployPath\Views\" -Recurse -Force
Copy-Item "wwwroot" -Destination "$deployPath\wwwroot\" -Recurse -Force
Copy-Item "web.config" -Destination "$deployPath\web.config" -Force
Copy-Item "Global.asax" -Destination "$deployPath\Global.asax" -Force
Copy-Item "App_Start" -Destination "$deployPath\App_Start\" -Recurse -Force

Write-Host "Deployment dosyaları $deployPath klasörüne hazırlandı!"
Write-Host "Bu klasördeki dosyaları hosting'e yükleyin."
```

---

## ✅ DEPLOYMENT KONTROL LİSTESİ

- [ ] Visual Studio'da Release modda build yapıldı
- [ ] `bin/Release/` klasöründe DLL'ler var
- [ ] `web.config` connection string hosting'e göre güncellendi
- [ ] `Views/` klasörü hazır
- [ ] `wwwroot/` klasörü hazır
- [ ] `Global.asax` dosyası var
- [ ] Dosyalar hosting'e yüklendi
- [ ] IIS Application Pool .NET Framework 4.8'e ayarlandı
- [ ] Veritabanı oluşturuldu
- [ ] Uygulama test edildi

---

## 🆘 ACİL DURUM: Hala 500 Hatası Alıyorsanız

1. **IIS Log Dosyalarını Okuyun**
   - Plesk panelinde **Logs** bölümüne gidin
   - Son error log'ları okuyun
   - Hangi DLL veya dosya eksik görebilirsiniz

2. **web.config'de Debug Modunu Açın**
   ```xml
   <system.web>
     <compilation debug="true" targetFramework="4.8" />
   ```
   Bu sayede daha detaylı hata mesajları görebilirsiniz.

3. **Custom Error'ları Kapatın** (geçici olarak)
   ```xml
   <system.web>
     <customErrors mode="Off" />
   </system.web>
   ```

4. **Hosting Desteğine Başvurun**
   - Hangi .NET Framework versiyonları yüklü?
   - IIS Application Pool ayarları doğru mu?
   - SQL Server bağlantısı çalışıyor mu?

---

## 📞 DESTEK

Sorun devam ederse:
1. IIS log dosyalarını paylaşın
2. web.config dosyasını paylaşın (şifreleri gizleyerek)
3. Hangi adımda hata aldığınızı belirtin

