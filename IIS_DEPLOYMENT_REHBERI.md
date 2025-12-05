# 🚀 IIS Deployment Rehberi - ASP.NET Core Runtime Hatası Çözümü

## ❌ Hata: HTTP Error 500.31 - Failed to load ASP.NET Core runtime

Bu hata, IIS sunucusunda .NET 8.0 runtime'ın yüklü olmamasından kaynaklanır.

---

## ✅ ÇÖZÜM 1: Self-Contained Deployment (ÖNERİLEN)

Bu yöntemde runtime uygulamayla birlikte yüklenir, sunucuda .NET yüklü olması gerekmez.

### Adım 1: Self-Contained Publish

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
```

### Adım 2: Dosyaları Yükle

`publish/` klasöründeki **TÜM** dosyaları hosting'e yükleyin.

### Adım 3: web.config Kontrolü

`web.config` dosyasında şu ayarlar olmalı:
```xml
<aspNetCore processPath=".\RandevuWeb.exe" 
            arguments="" 
            stdoutLogEnabled="true" 
            stdoutLogFile=".\logs\stdout" 
            hostingModel="inprocess">
```

### Avantajları:
- ✅ Sunucuda .NET runtime yüklü olması gerekmez
- ✅ Daha güvenilir
- ✅ Bağımsız çalışır

### Dezavantajları:
- ❌ Dosya boyutu daha büyük (~100-150 MB)
- ❌ Yükleme süresi daha uzun

---

## ✅ ÇÖZÜM 2: Framework-Dependent Deployment

Bu yöntemde sunucuda .NET 8.0 runtime yüklü olmalıdır.

### Adım 1: Sunucuya .NET 8.0 Runtime Yükleyin

1. [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) indirin
2. Sunucuya yükleyin
3. IIS'i yeniden başlatın

### Adım 2: Framework-Dependent Publish

```powershell
dotnet publish -c Release -o ./publish
```

### Adım 3: web.config Güncellemesi

`web.config` dosyasında şu ayarlar olmalı:
```xml
<aspNetCore processPath="dotnet" 
            arguments=".\RandevuWeb.dll" 
            stdoutLogEnabled="true" 
            stdoutLogFile=".\logs\stdout" 
            hostingModel="inprocess">
```

### Avantajları:
- ✅ Daha küçük dosya boyutu (~10-20 MB)
- ✅ Daha hızlı yükleme

### Dezavantajları:
- ❌ Sunucuda .NET 8.0 runtime yüklü olmalı
- ❌ Runtime güncellemeleri manuel yapılmalı

---

## 🔍 Hata Ayıklama (Debugging)

### 1. Stdout Log'larını Aktifleştirin

`web.config` dosyasında:
```xml
stdoutLogEnabled="true"
stdoutLogFile=".\logs\stdout"
```

### 2. Logs Klasörü Oluşturun

Hosting'de `logs` klasörünü oluşturun ve yazma izni verin.

### 3. Log Dosyalarını Kontrol Edin

`logs/stdout_*.log` dosyalarını kontrol ederek hatayı görebilirsiniz.

---

## 📋 Kontrol Listesi

### Self-Contained Deployment için:
- [ ] `dotnet publish -c Release -r win-x64 --self-contained true` komutu çalıştırıldı
- [ ] `publish/` klasöründeki tüm dosyalar yüklendi
- [ ] `web.config` dosyasında `processPath=".\RandevuWeb.exe"` ayarı var
- [ ] `logs` klasörü oluşturuldu ve yazma izni verildi
- [ ] `appsettings.json` production connection string ile güncellendi

### Framework-Dependent Deployment için:
- [ ] Sunucuda .NET 8.0 Runtime yüklü
- [ ] `dotnet publish -c Release` komutu çalıştırıldı
- [ ] `publish/` klasöründeki dosyalar yüklendi
- [ ] `web.config` dosyasında `processPath="dotnet"` ayarı var
- [ ] IIS yeniden başlatıldı

---

## 🆘 Yaygın Sorunlar ve Çözümleri

### Sorun 1: "RandevuWeb.exe bulunamadı"
**Çözüm:** Self-contained publish yapıldığından emin olun ve tüm dosyaları yükleyin.

### Sorun 2: "Access Denied"
**Çözüm:** 
- IIS Application Pool identity'ye yazma izni verin
- `logs` klasörüne yazma izni verin
- `wwwroot` klasörüne yazma izni verin

### Sorun 3: "Connection String Hatası"
**Çözüm:** 
- `appsettings.json` dosyasını kontrol edin
- Connection string'in doğru olduğundan emin olun
- SQL Server'ın çalıştığından emin olun

### Sorun 4: "Migration Hatası"
**Çözüm:**
- `Migrations/` klasörünün yüklendiğinden emin olun
- Veritabanının oluşturulduğundan emin olun
- `database-script.sql` dosyasını çalıştırın

---

## 📝 Örnek Publish Komutları

### Self-Contained (Önerilen):
```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
```

### Framework-Dependent:
```powershell
dotnet publish -c Release -o ./publish
```

### Trimmed (Daha küçük boyut):
```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishTrimmed=true -o ./publish
```

---

## 🎯 Hızlı Çözüm (Self-Contained)

1. **Publish Et:**
   ```powershell
   dotnet publish -c Release -r win-x64 --self-contained true -o ./publish
   ```

2. **appsettings.json Oluştur:**
   `publish/appsettings.json` dosyasını production ayarları ile oluşturun.

3. **Dosyaları Yükle:**
   `publish/` klasöründeki tüm dosyaları hosting'e yükleyin.

4. **Logs Klasörü Oluştur:**
   Hosting'de `logs` klasörünü oluşturun.

5. **Test Et:**
   Uygulamayı tarayıcıda açın.

---

## 📞 Destek

Sorun devam ederse:
1. `logs/stdout_*.log` dosyalarını kontrol edin
2. IIS Event Viewer'ı kontrol edin
3. `web.config` dosyasını kontrol edin
4. Connection string'i kontrol edin

