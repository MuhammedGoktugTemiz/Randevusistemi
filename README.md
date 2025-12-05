# Randevu Sistemi - Web Uygulaması

Diş hekimi randevu yönetim sistemi. **.NET Framework 4.8** ve **ASP.NET MVC 5** ile geliştirilmiştir.

## Özellikler

- 🔐 Basit kullanıcı adı/şifre ile giriş
- 📊 Dashboard (Haftalık takvim + doktor kartları)
- 📅 Aylık takvim görünümü
- ➕ Randevu ekleme (diş şeması ile)
- 👥 Hasta yönetimi (ekleme, düzenleme, silme)
- 👨‍⚕️ Doktor yönetimi
- 📈 Raporlar ve istatistikler
- 🔔 Bildirimler (24 saat içindeki randevular)
- ⚙️ Ayarlar
- 📱 WhatsApp bildirimleri (opsiyonel)

## Teknolojiler

- **.NET Framework 4.8**
- **ASP.NET MVC 5**
- **Entity Framework 6**
- **SQL Server** (veritabanı)
- **Unity Dependency Injection**
- **BCrypt** şifre hash'leme
- **Forms Authentication**

## Sistem Gereksinimleri

- Windows Server (IIS)
- .NET Framework 4.8
- SQL Server (2012 veya üzeri)
- IIS 7.0 veya üzeri

## Kurulum (Development)

1. Visual Studio 2019 veya üzeri gerekli
2. .NET Framework 4.8 SDK yüklü olmalı
3. SQL Server yüklü ve çalışıyor olmalı
4. Projeyi Visual Studio'da açın
5. `web.config` dosyasındaki connection string'i güncelleyin
6. **Build** → **Rebuild Solution**
7. Projeyi çalıştırın (F5)

## Production Deployment

**ÖNEMLİ:** Bu proje .NET Framework 4.8 için geliştirilmiştir. ASP.NET Core gibi `dotnet publish` komutu çalışmaz!

Detaylı deployment rehberi için:
- `PRODUCTION_DEPLOYMENT_FINAL.md` - Tam deployment rehberi
- `NET_FRAMEWORK_4.8_DEPLOYMENT.md` - Detaylı adımlar

### Hızlı Deployment

1. Visual Studio'da **Release** modda build et
2. `bin/Release/` klasöründeki TÜM dosyaları → hosting/bin/
3. `Views/` klasörünü → hosting/Views/
4. `wwwroot/` klasörünü → hosting/wwwroot/
5. `App_Start/` klasörünü → hosting/App_Start/
6. `web.config` (connection string güncellenmiş) → hosting kök dizini
7. `Global.asax` → hosting kök dizini
8. IIS Application Pool → .NET Framework 4.8
9. Veritabanını oluştur (`database-script.sql`)

## Giriş Bilgileri

Varsayılan giriş bilgileri `web.config` dosyasında yapılandırılır:

```xml
<appSettings>
  <add key="DefaultUser:Username" value="admin" />
  <add key="DefaultUser:Password" value="Admin123.*" />
</appSettings>
```

**Güvenlik:** Şifreler BCrypt ile hash'lenmiştir.

## Veri Depolama

Tüm veriler **SQL Server** veritabanında saklanır:
- Entity Framework 6 ile yönetilir
- Connection string `web.config` dosyasında yapılandırılır

**ÖNEMLİ:** `web.config` dosyası hassas bilgiler içerir ve Git'e commit edilmemelidir!

## Proje Yapısı

```
RandevuWeb/
├── Controllers/        # MVC Controller'lar
├── Models/            # Veri modelleri
├── Services/          # Veri servisleri
├── Data/              # Entity Framework DbContext
├── Views/             # Razor view'lar
├── wwwroot/           # Statik dosyalar (CSS, JS)
├── App_Start/         # MVC yapılandırmaları
│   ├── BundleConfig.cs
│   ├── FilterConfig.cs
│   ├── RouteConfig.cs
│   └── UnityConfig.cs
├── Global.asax        # Application lifecycle
├── Global.asax.cs     # Application startup
├── web.config         # IIS ve uygulama yapılandırması
└── packages.config    # NuGet paket referansları
```

## Diş Şeması

Randevu ekleme sayfasında interaktif diş şeması bulunur:
- Üst çene: 18-28
- Alt çene: 48-38, 31-41
- Diş seçimi ile otomatik işlem türü aktifleşir

## Güvenlik Özellikleri

- ✅ BCrypt şifre hash'leme
- ✅ Rate limiting (brute force koruması)
- ✅ Forms Authentication
- ✅ Session yönetimi
- ✅ Git güvenliği (.gitignore yapılandırması)

## Dependency Injection

Proje **Unity Container** kullanarak Dependency Injection yapılandırması içerir:
- `App_Start/UnityConfig.cs` - DI yapılandırması
- Tüm servisler ve controller'lar Unity ile yönetilir

## WhatsApp Entegrasyonu

WhatsApp bildirimleri için `web.config` dosyasında yapılandırma:

```xml
<appSettings>
  <add key="WhatsApp:AccessToken" value="YOUR_WHATSAPP_ACCESS_TOKEN" />
  <add key="WhatsApp:PhoneNumberId" value="YOUR_PHONE_NUMBER_ID" />
</appSettings>
```

## Geliştirme

Visual Studio'da projeyi geliştirme modunda çalıştırmak için:
- **F5** tuşuna basın veya **Debug** → **Start Debugging**

## Production Ayarları

Production için `web.config` dosyasında:

```xml
<compilation debug="false" targetFramework="4.8" />
<customErrors mode="RemoteOnly" defaultRedirect="~/Home/Error">
  <error statusCode="404" redirect="~/Home/Error" />
  <error statusCode="500" redirect="~/Home/Error" />
</customErrors>
```

## Sorun Giderme

### HTTP 500 Hatası

1. IIS log dosyalarını kontrol edin
2. `bin/` klasöründe tüm DLL'lerin yüklü olduğundan emin olun
3. Connection string'i kontrol edin
4. IIS Application Pool'un .NET Framework 4.8'e ayarlandığından emin olun

Detaylı sorun giderme için `NET_FRAMEWORK_4.8_DEPLOYMENT.md` dosyasına bakın.

## Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

## Destek

Sorun yaşarsanız:
1. `PRODUCTION_DEPLOYMENT_FINAL.md` dosyasını okuyun
2. IIS log dosyalarını kontrol edin
3. `web.config` dosyasını kontrol edin
