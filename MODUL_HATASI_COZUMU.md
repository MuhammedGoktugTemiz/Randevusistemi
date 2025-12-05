# 🔧 HTTP Error 500.21 - AspNetCoreModule Hatası Çözümü

## ❌ Hata: Handler "aspNetCore" has a bad module "AspNetCoreModule"

Bu hata, sunucuda **ASP.NET Core Module**'un yüklü olmadığını gösterir.

---

## ⚠️ ÖNEMLİ: Self-Contained Deployment Bile Modül Gerektirir

**Yanlış Bilgi:** Self-contained deployment yapınca modül gereksinimi kalkar.

**Doğru Bilgi:** Self-contained deployment yapsanız bile IIS'in ASP.NET Core uygulamasını çalıştırması için **AspNetCoreModule** veya **AspNetCoreModuleV2** modülü gereklidir.

---

## ✅ ÇÖZÜM SEÇENEKLERİ

### Çözüm 1: ASP.NET Core Hosting Bundle Yüklemek (ÖNERİLEN)

Sunucuya **Microsoft ASP.NET Core Hosting Bundle** yüklenmelidir:

1. [.NET 8.0 Hosting Bundle](https://dotnet.microsoft.com/download/dotnet/8.0) indirin
2. Sunucuya yükleyin
3. IIS'i yeniden başlatın

**Not:** Paylaşımlı hosting kullanıyorsanız, hosting sağlayıcınızdan ASP.NET Core desteği istemeniz gerekir.

---

### Çözüm 2: Hosting Sağlayıcınızdan ASP.NET Core Desteği İsteyin

Paylaşımlı hosting kullanıyorsanız:

1. Hosting sağlayıcınıza **ASP.NET Core 8.0** desteği olup olmadığını sorun
2. Eğer yoksa, ASP.NET Core desteği olan bir hosting planına geçin
3. Veya VPS/dedicated server kullanın

---

### Çözüm 3: Modül Versiyonunu Kontrol Edin

Plesk panelinde veya IIS'te modül versiyonunu kontrol edin:

1. **IIS Manager** → **Modules** bölümüne gidin
2. **AspNetCoreModule** veya **AspNetCoreModuleV2** arayın
3. Hangi versiyon yüklüyse, `web.config`'de o versiyonu kullanın

**AspNetCoreModuleV2 varsa:**
```xml
<add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
```

**AspNetCoreModule varsa:**
```xml
<add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
```

---

## 🔍 Modül Kontrolü

### Windows Server'da Modül Kontrolü:

```powershell
# IIS modüllerini listele
Get-WebGlobalModule | Where-Object {$_.Name -like "*AspNetCore*"}
```

### Plesk'te Modül Kontrolü:

1. **Tools & Settings** → **Server Components**
2. **ASP.NET Core Module** veya **ASP.NET Core Hosting Bundle** arayın

---

## 📋 Self-Contained Deployment Adımları (Modül Yüklüyse)

Modül yüklüyse, self-contained deployment yapın:

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
```

Sonra `publish/` klasöründeki dosyaları hosting'e yükleyin.

---

## 🆘 Modül Yüklü Değilse Ne Yapmalı?

### Seçenek 1: Hosting Sağlayıcınızdan İsteyin

Hosting sağlayıcınıza şu mesajı gönderin:

> "Merhaba, ASP.NET Core 8.0 uygulamamı çalıştırmak için sunucuda ASP.NET Core Hosting Bundle yüklü olması gerekiyor. Lütfen ASP.NET Core 8.0 desteği ekleyebilir misiniz?"

### Seçenek 2: Alternatif Hosting Bulun

ASP.NET Core desteği olan hosting sağlayıcıları:
- Azure App Service
- AWS Elastic Beanstalk
- DigitalOcean App Platform
- Heroku (.NET buildpack ile)

### Seçenek 3: VPS/Dedicated Server Kullanın

Kendi sunucunuzu yönetiyorsanız, ASP.NET Core Hosting Bundle'ı kendiniz yükleyebilirsiniz.

---

## ✅ Kontrol Listesi

- [ ] Sunucuda ASP.NET Core Module yüklü mü? (IIS Manager'dan kontrol edin)
- [ ] Self-contained deployment yaptınız mı?
- [ ] `web.config` dosyasında doğru modül adı var mı?
- [ ] IIS yeniden başlatıldı mı?
- [ ] Hosting sağlayıcınızdan ASP.NET Core desteği istediniz mi?

---

## 📝 web.config Örnekleri

### AspNetCoreModuleV2 için (Önerilen):
```xml
<handlers>
  <remove name="aspNetCore" />
  <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
</handlers>
<aspNetCore processPath=".\RandevuWeb.exe" 
            arguments="" 
            stdoutLogEnabled="true" 
            stdoutLogFile=".\logs\stdout" 
            hostingModel="inprocess">
</aspNetCore>
```

### AspNetCoreModule için (Eski sürümler):
```xml
<handlers>
  <remove name="aspNetCore" />
  <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
</handlers>
<aspNetCore processPath=".\RandevuWeb.exe" 
            arguments="" 
            stdoutLogEnabled="true" 
            stdoutLogFile=".\logs\stdout" 
            hostingModel="inprocess">
</aspNetCore>
```

---

## 🎯 Hızlı Çözüm

1. **Hosting sağlayıcınızdan ASP.NET Core 8.0 desteği isteyin**
2. Modül yüklendikten sonra self-contained deployment yapın
3. `web.config`'de doğru modül adını kullanın
4. IIS'i yeniden başlatın

---

**ÖNEMLİ:** Self-contained deployment bile ASP.NET Core Module gerektirir. Modül olmadan IIS ASP.NET Core uygulamasını çalıştıramaz.

