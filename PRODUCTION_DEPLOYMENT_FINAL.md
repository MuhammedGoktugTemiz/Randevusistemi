# 🚀 Production Deployment - Final Rehber

## ✅ Proje Durumu

Proje **.NET Framework 4.8** ve **ASP.NET MVC 5** ile tamamen uyumlu hale getirilmiştir.

---

## 📋 DEPLOYMENT ADIMLARI

### 1️⃣ Visual Studio'da Build Et

```powershell
# Visual Studio'da:
1. Solution Configuration → Release seçin
2. Build → Rebuild Solution
3. Build başarılı olmalı (hata yoksa devam edin)
```

### 2️⃣ Hosting'e Yüklenecek Dosyalar

#### ✅ MUTLAKA YÜKLENMESİ GEREKEN:

```
📁 bin/Release/ klasöründeki TÜM dosyalar
   → hosting/bin/ klasörüne yükleyin
   
📁 Views/ klasörü (TAMAMEN)
   → hosting/Views/ klasörüne yükleyin
   
📁 wwwroot/ klasörü (TAMAMEN)
   → hosting/wwwroot/ klasörüne yükleyin
   
📁 App_Start/ klasörü (TAMAMEN)
   → hosting/App_Start/ klasörüne yükleyin
   
📄 web.config
   → hosting kök dizinine yükleyin (connection string'i güncelleyin!)
   
📄 Global.asax
   → hosting kök dizinine yükleyin
   
📄 packages.config
   → hosting kök dizinine yükleyin (opsiyonel)
```

### 3️⃣ web.config Connection String Güncelleme

**ÖNEMLİ:** Hosting'e yüklemeden önce `web.config` dosyasındaki connection string'i güncelleyin!

Plesk panelinden SQL Server bilgilerini alın ve şu şekilde güncelleyin:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=SQL_SERVER_ADRESI;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Örnek connection string'ler:**
- Plesk: `Server=localhost;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;`
- Remote SQL: `Server=sqlXXX.guzel.net.tr;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;`

### 4️⃣ IIS Application Pool Ayarları

Plesk panelinde veya IIS Manager'da:

1. **Application Pool** seçin
2. **.NET CLR Version** → **v4.0** seçin
3. **Managed Pipeline Mode** → **Integrated** seçin
4. **Start Mode** → **AlwaysRunning** (opsiyonel, performans için)

### 5️⃣ Veritabanını Oluştur

1. Plesk panelinden **Databases** → **SQL Server** bölümüne gidin
2. `database-script.sql` dosyasını çalıştırın
3. Veya SQL Server Management Studio ile bağlanıp script'i çalıştırın

### 6️⃣ Test Et

1. Tarayıcıda `randevu.dtomeralbayrak.com` adresini açın
2. Giriş sayfası görünmeli
3. Admin girişi yapın: `admin` / `Admin123.*`

---

## 🔧 PRODUCTION AYARLARI

### web.config Production Ayarları

```xml
<!-- Production için -->
<compilation debug="false" targetFramework="4.8" />
<customErrors mode="RemoteOnly" defaultRedirect="~/Home/Error">
  <error statusCode="404" redirect="~/Home/Error" />
  <error statusCode="500" redirect="~/Home/Error" />
</customErrors>
```

**Not:** Development için `debug="true"` ve `customErrors mode="Off"` yapabilirsiniz.

### Güvenlik Ayarları

- ✅ Şifreler BCrypt ile hash'leniyor
- ✅ Forms Authentication aktif
- ✅ Rate limiting (brute force koruması)
- ✅ HTTPS kullanımı önerilir (SSL sertifikası)

---

## 📦 DOSYA YAPISI

Hosting'deki dosya yapısı şöyle olmalı:

```
hosting/
├── bin/
│   ├── RandevuWeb.dll
│   ├── EntityFramework.dll
│   ├── EntityFramework.SqlServer.dll
│   ├── BCrypt.Net-Next.dll
│   ├── System.Web.Mvc.dll
│   ├── Unity.Container.dll
│   ├── Unity.Mvc5.dll
│   ├── Newtonsoft.Json.dll
│   └── ... (diğer dependency DLL'leri)
├── Views/
│   ├── Account/
│   ├── Appointment/
│   ├── Calendar/
│   ├── Doctor/
│   ├── DoctorAuth/
│   ├── Home/
│   ├── Notifications/
│   ├── Patient/
│   ├── Reports/
│   ├── Settings/
│   ├── Shared/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   └── web.config
├── wwwroot/
│   ├── css/
│   └── js/
├── App_Start/
│   ├── BundleConfig.cs
│   ├── FilterConfig.cs
│   ├── RouteConfig.cs
│   └── UnityConfig.cs
├── web.config
├── Global.asax
└── packages.config (opsiyonel)
```

---

## 🆘 SORUN GİDERME

### HTTP 500 Hatası

1. **IIS Log Dosyalarını Kontrol Et**
   - Plesk → Logs → Error Log
   - Hangi DLL eksik veya hata veriyor görebilirsiniz

2. **bin Klasörünü Kontrol Et**
   - Tüm DLL'ler yüklü mü?
   - Eksik DLL varsa Visual Studio'da tekrar build edin

3. **Connection String Kontrolü**
   - SQL Server adresi doğru mu?
   - Database adı doğru mu?
   - Kullanıcı adı ve şifre doğru mu?

4. **web.config Syntax Kontrolü**
   - XML syntax hatası var mı?
   - Tüm tag'ler kapatılmış mı?

### Veritabanı Bağlantı Hatası

1. Connection string'i kontrol edin
2. SQL Server'ın çalıştığından emin olun
3. Firewall ayarlarını kontrol edin
4. Kullanıcı adı ve şifrenin doğru olduğundan emin olun

### Sayfa Bulunamadı (404)

1. `Views/` klasörünün yüklendiğinden emin olun
2. `wwwroot/` klasörünün yüklendiğinden emin olun
3. Route yapılandırmasını kontrol edin

---

## ✅ DEPLOYMENT KONTROL LİSTESİ

- [ ] Visual Studio'da Release modda build yapıldı
- [ ] `bin/Release/` klasöründe tüm DLL'ler var
- [ ] `web.config` connection string hosting'e göre güncellendi
- [ ] `web.config` production ayarları yapıldı (debug=false)
- [ ] `Views/` klasörü hazır
- [ ] `wwwroot/` klasörü hazır
- [ ] `App_Start/` klasörü hazır
- [ ] `Global.asax` dosyası var
- [ ] Dosyalar hosting'e yüklendi
- [ ] IIS Application Pool .NET Framework 4.8'e ayarlandı
- [ ] Veritabanı oluşturuldu
- [ ] Uygulama test edildi
- [ ] Giriş yapılabiliyor
- [ ] Sayfalar açılıyor

---

## 🎯 HIZLI DEPLOYMENT

En hızlı yöntem:

1. Visual Studio → Build → Rebuild Solution (Release)
2. `bin/Release/` klasöründeki TÜM dosyaları → hosting/bin/
3. `Views/` klasörünü → hosting/Views/
4. `wwwroot/` klasörünü → hosting/wwwroot/
5. `App_Start/` klasörünü → hosting/App_Start/
6. `web.config` (connection string güncellenmiş) → hosting kök dizini
7. `Global.asax` → hosting kök dizini
8. Test et!

---

## 📞 DESTEK

Sorun devam ederse:
1. IIS log dosyalarını paylaşın
2. web.config dosyasını paylaşın (şifreleri gizleyerek)
3. Hangi adımda hata aldığınızı belirtin

