# 🚀 .NET Framework 4.8 Hızlı Deployment

## ⚡ HIZLI ÇÖZÜM (5 Dakika)

### 1. Visual Studio'da Build Et
```
1. Visual Studio'yu aç
2. Solution Configuration → Release seç
3. Build → Rebuild Solution
```

### 2. Hosting'e Yükle
Şu klasörleri ve dosyaları hosting'e yükle:

```
✅ bin/Release/ klasöründeki TÜM dosyalar → hosting/bin/ klasörüne
✅ Views/ klasörü → hosting/Views/ klasörüne  
✅ wwwroot/ klasörü → hosting/wwwroot/ klasörüne
✅ web.config → hosting kök dizinine (connection string'i güncelle!)
✅ Global.asax → hosting kök dizinine
```

### 3. web.config Connection String'i Güncelle

Plesk panelinden SQL Server bilgilerini al ve `web.config` dosyasındaki connection string'i güncelle:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=SQL_SERVER_ADRESI;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**ÖNEMLİ:** `Server=` kısmına hosting'inizin gerçek SQL Server adresini yazın!

### 4. IIS Application Pool Kontrolü

Plesk panelinde:
- Application Pool → .NET CLR Version: v4.0
- Managed Pipeline Mode: Integrated

### 5. Test Et

Tarayıcıda `randevu.dtomeralbayrak.com` adresini aç.

---

## 🔴 HALA 500 HATASI ALIYORSANIZ

### Adım 1: IIS Log Dosyalarını Oku
Plesk panelinde **Logs** bölümüne git ve son error log'ları oku. Hangi DLL eksik görebilirsin.

### Adım 2: web.config'de Custom Errors Kapat
`web.config` dosyasında:
```xml
<system.web>
  <customErrors mode="Off" />
</system.web>
```
Bu sayede detaylı hata mesajı görebilirsin.

### Adım 3: bin Klasörünü Kontrol Et
Hosting'deki `bin/` klasöründe şu DLL'ler olmalı:
- RandevuWeb.dll
- EntityFramework.dll
- EntityFramework.SqlServer.dll
- BCrypt.Net-Next.dll
- System.Web.Mvc.dll
- Unity.Container.dll
- Unity.Mvc5.dll
- Newtonsoft.Json.dll

Eksik DLL varsa, Visual Studio'da projeyi tekrar build et ve `bin/Release/` klasöründeki TÜM DLL'leri yükle.

---

## 📞 SORUN DEVAM EDERSE

1. IIS log dosyalarını paylaş
2. web.config dosyasını paylaş (şifreleri gizleyerek)
3. Hangi adımda hata aldığını belirt

