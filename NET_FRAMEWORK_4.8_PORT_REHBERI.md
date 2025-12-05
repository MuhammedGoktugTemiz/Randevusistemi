# 🔄 .NET Framework 4.8 Port Rehberi

## ✅ Tamamlanan İşlemler

Tüm kod .NET Framework 4.8'e port edildi. Aşağıdaki dosyalar oluşturuldu:

### 📁 Proje Dosyaları
- `RandevuWeb.NetFramework.csproj` - .NET Framework 4.8 proje dosyası
- `packages.config` - NuGet paketleri
- `Global.asax` ve `Global.asax.cs` - Uygulama başlangıç dosyaları
- `App_Start/RouteConfig.cs` - Routing yapılandırması
- `App_Start/FilterConfig.cs` - Filter yapılandırması
- `App_Start/BundleConfig.cs` - Bundle yapılandırması
- `App_Start/UnityConfig.cs` - Dependency Injection yapılandırması
- `Properties/AssemblyInfo.cs` - Assembly bilgileri

### 📁 Models (.NET Framework 4.8)
- `Models/Doctor.NetFramework.cs`
- `Models/Patient.NetFramework.cs`
- `Models/Appointment.NetFramework.cs`
- `Models/User.NetFramework.cs`
- `Models/DoctorStats.NetFramework.cs`

### 📁 Data (.NET Framework 4.8)
- `Data/ApplicationDbContext.NetFramework.cs` - Entity Framework 6

### 📁 Services (.NET Framework 4.8)
- `Services/IDataService.NetFramework.cs`
- `Services/SqlDataService.NetFramework.cs`
- `Services/IPasswordHasher.NetFramework.cs`
- `Services/BCryptPasswordHasher.NetFramework.cs`
- `Services/IWhatsAppService.NetFramework.cs`
- `Services/WhatsAppService.NetFramework.cs`

### 📁 Controllers (.NET Framework 4.8)
- `Controllers/HomeController.NetFramework.cs`
- `Controllers/AccountController.NetFramework.cs`
- `Controllers/AppointmentController.NetFramework.cs`
- `Controllers/PatientController.NetFramework.cs`
- `Controllers/DoctorController.NetFramework.cs`
- `Controllers/CalendarController.NetFramework.cs`
- `Controllers/DoctorAuthController.NetFramework.cs`
- `Controllers/NotificationsController.NetFramework.cs`
- `Controllers/ReportsController.NetFramework.cs`
- `Controllers/SettingsController.NetFramework.cs`

### 📁 Configuration
- `web.config.NetFramework` - Ana web.config
- `Views/web.config` - Views için web.config

---

## 🚀 Kurulum Adımları

### Adım 1: Yeni Proje Oluşturun

1. Visual Studio'yu açın
2. **File** → **New** → **Project**
3. **ASP.NET Web Application (.NET Framework)** seçin
4. **MVC** template'i seçin
5. **.NET Framework 4.8** seçin
6. Proje adı: `RandevuWeb`

### Adım 2: Dosyaları Kopyalayın

#### Models Klasörü:
- `Models/Doctor.NetFramework.cs` → `Models/Doctor.cs` olarak kopyalayın
- `Models/Patient.NetFramework.cs` → `Models/Patient.cs` olarak kopyalayın
- `Models/Appointment.NetFramework.cs` → `Models/Appointment.cs` olarak kopyalayın
- `Models/User.NetFramework.cs` → `Models/User.cs` olarak kopyalayın
- `Models/DoctorStats.NetFramework.cs` → `Models/DoctorStats.cs` olarak kopyalayın

#### Data Klasörü:
- `Data/ApplicationDbContext.NetFramework.cs` → `Data/ApplicationDbContext.cs` olarak kopyalayın

#### Services Klasörü:
- Tüm `Services/*.NetFramework.cs` dosyalarını `.cs` uzantısıyla kopyalayın

#### Controllers Klasörü:
- Tüm `Controllers/*.NetFramework.cs` dosyalarını `.cs` uzantısıyla kopyalayın

#### App_Start Klasörü:
- Tüm `App_Start/*.cs` dosyalarını kopyalayın

#### Views Klasörü:
- Mevcut Views klasörünü olduğu gibi kopyalayın (değişiklik yok)

#### wwwroot Klasörü:
- Mevcut wwwroot klasörünü olduğu gibi kopyalayın

### Adım 3: Configuration Dosyalarını Kopyalayın

- `web.config.NetFramework` → `web.config` olarak kopyalayın
- `Views/web.config` → `Views/web.config` olarak kopyalayın
- `Global.asax` ve `Global.asax.cs` → Proje köküne kopyalayın
- `packages.config` → Proje köküne kopyalayın
- `Properties/AssemblyInfo.cs` → `Properties/AssemblyInfo.cs` olarak kopyalayın

### Adım 4: NuGet Paketlerini Yükleyin

**Package Manager Console'da:**

```powershell
Install-Package EntityFramework -Version 6.5.1
Install-Package Microsoft.AspNet.Mvc -Version 5.2.8
Install-Package Microsoft.AspNet.Web.Optimization -Version 1.1.3
Install-Package Microsoft.AspNet.WebApi -Version 5.2.9
Install-Package Unity.Mvc5 -Version 5.11.2
Install-Package BCrypt.Net-Next -Version 4.0.3
Install-Package Newtonsoft.Json -Version 13.0.3
```

**Veya packages.config dosyasını kullanarak:**

```powershell
Update-Package -reinstall
```

### Adım 5: Proje Dosyasını Güncelleyin

`RandevuWeb.NetFramework.csproj` dosyasını projenize ekleyin veya mevcut `.csproj` dosyasını güncelleyin.

### Adım 6: Connection String'i Güncelleyin

`web.config` dosyasındaki connection string'i kendi SQL Server bilgilerinize göre güncelleyin:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=localhost\MSSQLSERVER2022;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Adım 7: App Settings'i Güncelleyin

`web.config` dosyasındaki `<appSettings>` bölümünü güncelleyin:

```xml
<appSettings>
  <add key="DefaultUser:Username" value="admin" />
  <add key="DefaultUser:Password" value="Admin123.*" />
  <add key="WhatsApp:AccessToken" value="YOUR_WHATSAPP_ACCESS_TOKEN" />
  <add key="WhatsApp:PhoneNumberId" value="YOUR_PHONE_NUMBER_ID" />
</appSettings>
```

### Adım 8: Veritabanını Oluşturun

SQL Server Management Studio ile `database-script.sql` dosyasını çalıştırın.

### Adım 9: Build ve Test

1. **Build** → **Build Solution**
2. Hataları kontrol edin ve düzeltin
3. **F5** ile çalıştırın

---

## 🔄 Ana Değişiklikler

### 1. Namespace Değişiklikleri
- `namespace RandevuWeb;` → `namespace RandevuWeb { }`
- File-scoped namespace'ler kaldırıldı

### 2. Using Statements
- `Microsoft.AspNetCore.*` → `System.Web.*`
- `Microsoft.EntityFrameworkCore` → `System.Data.Entity`
- `Microsoft.Extensions.*` → `System.Web.*` veya kaldırıldı

### 3. Controllers
- `IActionResult` → `ActionResult`
- `Task<IActionResult>` → `ActionResult` (async kaldırıldı)
- `HttpContext` → `Request`, `Response`, `Session`
- `IConfiguration` → `WebConfigurationManager.AppSettings`
- `IMemoryCache` → `Session`
- `IFormFile` → `HttpPostedFileBase`

### 4. Authentication
- `CookieAuthenticationDefaults` → `FormsAuthentication`
- `HttpContext.SignInAsync` → `FormsAuthentication.SetAuthCookie`
- `HttpContext.SignOutAsync` → `FormsAuthentication.SignOut`
- `ClaimsIdentity` → Session kullanımı

### 5. Dependency Injection
- `builder.Services` → Unity Container
- Constructor injection → `DependencyResolver.Current.GetService<T>()`

### 6. Entity Framework
- `DbContext` → `DbContext` (aynı ama EF6)
- `OnModelCreating(ModelBuilder)` → `OnModelCreating(DbModelBuilder)`
- `HasOne().WithMany()` → `HasRequired().WithMany()`
- `OnDelete(DeleteBehavior.Restrict)` → `WillCascadeOnDelete(false)`

### 7. File Upload
- `IFormFile` → `HttpPostedFileBase`
- `Directory.GetCurrentDirectory()` → `Server.MapPath()`

### 8. JSON
- `System.Text.Json` → `Newtonsoft.Json`
- `JsonSerializer.Serialize` → `JsonConvert.SerializeObject`

---

## 📋 Kontrol Listesi

- [ ] Yeni ASP.NET MVC 5 projesi oluşturuldu
- [ ] Tüm `.NetFramework.cs` dosyaları `.cs` olarak kopyalandı
- [ ] `web.config.NetFramework` → `web.config` kopyalandı
- [ ] `Global.asax` ve `Global.asax.cs` kopyalandı
- [ ] `App_Start` klasörü oluşturuldu ve dosyalar kopyalandı
- [ ] NuGet paketleri yüklendi
- [ ] Connection string güncellendi
- [ ] App settings güncellendi
- [ ] Veritabanı oluşturuldu
- [ ] Build başarılı
- [ ] Uygulama çalışıyor

---

## 🆘 Sorun Giderme

### Build Hataları

1. **Missing References:**
   - NuGet paketlerini yeniden yükleyin
   - `packages.config` dosyasını kontrol edin

2. **Namespace Hataları:**
   - File-scoped namespace'leri normal namespace'e çevirin
   - `using` statement'ları kontrol edin

3. **Entity Framework Hataları:**
   - Entity Framework 6.5.1'in yüklü olduğundan emin olun
   - Connection string'i kontrol edin

### Runtime Hataları

1. **Database Connection:**
   - Connection string'i kontrol edin
   - SQL Server'ın çalıştığından emin olun

2. **Dependency Injection:**
   - UnityConfig.cs dosyasını kontrol edin
   - Unity.Mvc5 paketinin yüklü olduğundan emin olun

3. **Authentication:**
   - Forms Authentication'ın aktif olduğundan emin olun
   - Session state'in aktif olduğundan emin olun

---

## 📝 Önemli Notlar

1. **Views:** Views dosyaları değişmedi, olduğu gibi kullanılabilir
2. **wwwroot:** Statik dosyalar olduğu gibi kullanılabilir
3. **JavaScript:** JavaScript dosyaları değişmedi
4. **CSS:** CSS dosyaları değişmedi

---

## 🎯 Hızlı Başlangıç

1. Visual Studio'da yeni ASP.NET MVC 5 projesi oluşturun
2. Tüm `.NetFramework.cs` dosyalarını `.cs` olarak kopyalayın
3. `web.config.NetFramework` → `web.config` kopyalayın
4. NuGet paketlerini yükleyin
5. Connection string'i güncelleyin
6. Build edin ve çalıştırın

---

**Başarılar!** 🎉

