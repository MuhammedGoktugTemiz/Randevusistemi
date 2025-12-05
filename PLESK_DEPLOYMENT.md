# 🚀 Plesk Deployment Rehberi

## ⚠️ Plesk 4.8 Uyumluluk Sorunları

Plesk 4.8 çok eski bir sürümdür ve modern ASP.NET Core uygulamalarıyla uyumlu olmayabilir. Bu rehber Plesk için özel ayarları içerir.

---

## ✅ Plesk için Özel Ayarlar

### 1. web.config Dosyasını Güncelleyin

Plesk için özel `web.config` dosyası kullanın (`web.config.plesk` dosyasını `web.config` olarak kopyalayın):

**Önemli Değişiklikler:**
- `AspNetCoreModuleV2` yerine `AspNetCoreModule` kullanılır (eski Plesk sürümleri için)
- `forwardWindowsAuthToken="false"` eklendi
- `ASPNETCORE_PATH` environment variable eklendi
- Basitleştirilmiş yapı

### 2. ASP.NET Core Module Kontrolü

Plesk panelinde ASP.NET Core Module'un yüklü olduğundan emin olun:

1. **Plesk Panel** → **Tools & Settings** → **Server Components**
2. **ASP.NET Core Module** veya **ASP.NET Core Hosting Bundle** arayın
3. Yüklü değilse, sunucu yöneticisinden yükletmeniz gerekebilir

### 3. Application Pool Ayarları

Plesk panelinde Application Pool ayarlarını kontrol edin:

1. **Websites & Domains** → **randevu.dtomeralbayrak.com** → **ASP.NET Settings**
2. **Application Pool** ayarlarını kontrol edin:
   - **.NET CLR Version:** No Managed Code (ASP.NET Core için)
   - **Managed Pipeline Mode:** Integrated
   - **Identity:** ApplicationPoolIdentity veya özel kullanıcı

### 4. Plesk'te Dosya İzinleri

Plesk panelinden dosya izinlerini ayarlayın:

1. **File Manager** ile `logs` klasörünü oluşturun
2. **logs** klasörüne sağ tıklayın → **Change Permissions**
3. **Write** iznini verin (755 veya 777)

---

## 🔧 Alternatif Çözümler

### Çözüm 1: Out-of-Process Hosting

Eğer in-process hosting çalışmıyorsa, out-of-process deneyin:

```xml
<aspNetCore processPath="dotnet" 
            arguments=".\RandevuWeb.dll" 
            hostingModel="outofprocess">
```

**Not:** Bu durumda sunucuda .NET 8.0 Runtime yüklü olmalıdır.

### Çözüm 2: Standalone Deployment

Self-contained deployment kullanın (önerilen):

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
```

### Çözüm 3: Framework-Dependent Deployment

Eğer sunucuda .NET 8.0 Runtime varsa:

```powershell
dotnet publish -c Release -o ./publish
```

Ve `web.config`'de:

```xml
<aspNetCore processPath="dotnet" 
            arguments=".\RandevuWeb.dll" 
            hostingModel="inprocess">
```

---

## 📋 Plesk Deployment Adımları

### Adım 1: Dosyaları Hazırlayın

```powershell
# Self-contained publish
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
```

### Adım 2: web.config'i Güncelleyin

`web.config.plesk` dosyasını `publish/web.config` olarak kopyalayın.

### Adım 3: appsettings.json Oluşturun

`publish/appsettings.json` dosyasını production ayarları ile oluşturun.

### Adım 4: Plesk'e Yükleyin

1. **File Manager** ile `httpdocs` klasörüne gidin
2. `publish/` klasöründeki **TÜM** dosyaları yükleyin
3. `logs` klasörünü oluşturun ve yazma izni verin

### Adım 5: Plesk Ayarlarını Yapın

1. **Websites & Domains** → **randevu.dtomeralbayrak.com**
2. **ASP.NET Settings** → **ASP.NET Core** seçin
3. **Application Pool** ayarlarını kontrol edin

### Adım 6: Veritabanını Oluşturun

1. **Databases** → **Microsoft SQL Server**
2. `database-script.sql` dosyasını çalıştırın

---

## 🆘 Plesk'e Özgü Sorunlar

### Sorun 1: "AspNetCoreModule not found"

**Çözüm:**
- Plesk panelinde ASP.NET Core Module'un yüklü olduğundan emin olun
- Sunucu yöneticisinden ASP.NET Core Hosting Bundle yükletmeniz gerekebilir

### Sorun 2: "Access Denied"

**Çözüm:**
- Plesk File Manager ile `logs` klasörüne yazma izni verin
- Application Pool identity'ye izin verin

### Sorun 3: "Process Path Not Found"

**Çözüm:**
- `RandevuWeb.exe` dosyasının mevcut olduğundan emin olun
- Self-contained deployment yaptığınızdan emin olun
- Dosya yolunu kontrol edin (mutlak yol gerekebilir)

### Sorun 4: "Connection String Error"

**Çözüm:**
- Plesk panelinde SQL Server connection string'i kontrol edin
- `appsettings.json` dosyasının doğru yerde olduğundan emin olun
- Connection string'de `MultipleActiveResultSets=True` ekleyin

---

## 📝 Plesk web.config Örneği

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <remove name="aspNetCore" />
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath=".\RandevuWeb.exe" 
                  arguments="" 
                  stdoutLogEnabled="true" 
                  stdoutLogFile=".\logs\stdout" 
                  hostingModel="inprocess">
        <environmentVariables>
          <environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
        </environmentVariables>
      </aspNetCore>
      <httpErrors errorMode="Detailed" />
    </system.webServer>
  </location>
</configuration>
```

---

## 🔍 Plesk Log Dosyaları

Plesk'te log dosyaları genellikle şu konumlarda bulunur:

- **IIS Logs:** `C:\inetpub\logs\LogFiles\`
- **Application Logs:** `logs/stdout_*.log` (uygulama klasöründe)
- **Plesk Logs:** Plesk panelinde **Logs** bölümünden erişilebilir

---

## ✅ Kontrol Listesi

- [ ] `web.config.plesk` dosyasını `web.config` olarak kopyaladınız
- [ ] Self-contained deployment yaptınız
- [ ] `appsettings.json` dosyasını oluşturdunuz
- [ ] `logs` klasörünü oluşturdunuz ve yazma izni verdiniz
- [ ] Plesk panelinde ASP.NET Core ayarlarını yaptınız
- [ ] Application Pool ayarlarını kontrol ettiniz
- [ ] Veritabanını oluşturdunuz

---

## 🎯 Hızlı Çözüm

1. **web.config.plesk** dosyasını `web.config` olarak kopyalayın
2. Self-contained publish yapın
3. Plesk'e yükleyin
4. `logs` klasörünü oluşturun
5. `appsettings.json` dosyasını oluşturun
6. Plesk panelinde ASP.NET Core ayarlarını yapın

---

**Not:** Plesk 4.8 çok eski bir sürümdür. Mümkünse Plesk'i güncellemeyi düşünün veya hosting sağlayıcınızdan ASP.NET Core desteği hakkında bilgi alın.

