# 🚀 .NET Framework 4.8 Kurulum Rehberi - Hızlı Başlangıç

## ⚡ Hızlı Kurulum (5 Adım)

### 1. Visual Studio'da Yeni Proje Oluşturun

```
File → New → Project → ASP.NET Web Application (.NET Framework)
Template: MVC
Framework: .NET Framework 4.8
```

### 2. Dosyaları Kopyalayın

**Tüm `.NetFramework.cs` dosyalarını `.cs` uzantısıyla kopyalayın:**

```powershell
# Models
Copy Models\Doctor.NetFramework.cs Models\Doctor.cs
Copy Models\Patient.NetFramework.cs Models\Patient.cs
Copy Models\Appointment.NetFramework.cs Models\Appointment.cs
Copy Models\User.NetFramework.cs Models\User.cs
Copy Models\DoctorStats.NetFramework.cs Models\DoctorStats.cs

# Data
Copy Data\ApplicationDbContext.NetFramework.cs Data\ApplicationDbContext.cs

# Services
Copy Services\*.NetFramework.cs Services\*.cs

# Controllers
Copy Controllers\*.NetFramework.cs Controllers\*.cs

# Config
Copy web.config.NetFramework web.config
Copy Global.asax* .
Copy App_Start\*.* App_Start\
Copy packages.config .
```

### 3. NuGet Paketlerini Yükleyin

**Package Manager Console:**

```powershell
Install-Package EntityFramework -Version 6.5.1
Install-Package Microsoft.AspNet.Mvc -Version 5.2.8
Install-Package Unity.Mvc5 -Version 5.11.2
Install-Package BCrypt.Net-Next -Version 4.0.3
Install-Package Newtonsoft.Json -Version 13.0.3
```

### 4. web.config'i Güncelleyin

Connection string ve app settings'i kendi bilgilerinize göre güncelleyin.

### 5. Build ve Çalıştır

**F5** ile çalıştırın!

---

## 📝 Dosya Eşleştirmeleri

| .NET Core Dosyası | .NET Framework Dosyası |
|-------------------|------------------------|
| `Program.cs` | `Global.asax.cs` |
| `appsettings.json` | `web.config` (appSettings) |
| `Microsoft.AspNetCore.*` | `System.Web.*` |
| `EntityFrameworkCore` | `EntityFramework` |
| `IActionResult` | `ActionResult` |
| `IFormFile` | `HttpPostedFileBase` |

---

**Detaylı bilgi için `NET_FRAMEWORK_4.8_PORT_REHBERI.md` dosyasına bakın.**

