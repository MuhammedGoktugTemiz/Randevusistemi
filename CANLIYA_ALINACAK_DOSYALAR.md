# 📦 Canlıya Alınacak Dosyalar - Detaylı Liste

## 🚀 YÖNTEM 1: Otomatik Publish (ÖNERİLEN)

### Adım 1: Publish Komutu Çalıştır
```powershell
dotnet publish -c Release -o ./publish
```

### Adım 2: Publish Klasöründeki TÜM Dosyaları Yükle
`publish/` klasöründeki **TÜM** dosyaları hosting'e yükleyin.

---

## 📋 YÖNTEM 2: Manuel Dosya Listesi

### ✅ MUTLAKA YÜKLENMESİ GEREKEN DOSYALAR

#### 1. Ana Uygulama Dosyaları
```
✅ RandevuWeb.dll                    # Ana uygulama DLL'i
✅ RandevuWeb.exe                     # (varsa)
✅ web.config                         # IIS konfigürasyonu
✅ appsettings.json                   # Production connection string ile
✅ appsettings.Production.json        # Production ayarları
```

#### 2. Views Klasörü (TAMAMEN)
```
✅ Views/
   ✅ _ViewImports.cshtml
   ✅ _ViewStart.cshtml
   ✅ Account/
   ✅ Appointment/
   ✅ Calendar/
   ✅ Doctor/
   ✅ DoctorAuth/
   ✅ Home/
   ✅ Notifications/
   ✅ Patient/
   ✅ Reports/
   ✅ Settings/
   ✅ Shared/
```

#### 3. wwwroot Klasörü (TAMAMEN)
```
✅ wwwroot/
   ✅ css/
   ✅ js/
   ✅ (varsa images/, fonts/, vb.)
```

#### 4. Migrations Klasörü (EF Core için gerekli)
```
✅ Migrations/
   ✅ 20251204102053_InitialCreate.cs
   ✅ 20251204102053_InitialCreate.Designer.cs
   ✅ ApplicationDbContextModelSnapshot.cs
```

#### 5. .NET Runtime Dosyaları (publish klasöründen)
```
✅ *.dll                              # Tüm DLL dosyaları
✅ *.deps.json                        # Dependency dosyası
✅ *.runtimeconfig.json               # Runtime konfigürasyonu
✅ *.pdb                              # Debug symbols (opsiyonel)
```

---

### ❌ YÜKLENMEMESİ GEREKEN DOSYALAR

#### 1. Development Script'leri
```
❌ *.ps1                              # PowerShell script'leri
❌ create-migration.ps1
❌ update-database.ps1
❌ test-sql-connection.ps1
❌ fix-sql-authentication.ps1
❌ git-commit.ps1
❌ create-sql-script.ps1
```

#### 2. SQL Script'leri (Sadece veritabanı kurulumu için)
```
❌ database-script.sql                # Sadece SQL Server'da çalıştırılacak
❌ enable-sa-login.sql
❌ create-backup.sql
❌ create-backup-simple.sql
❌ restore-backup.sql
```

#### 3. Build ve Cache Dosyaları
```
❌ bin/                               # Build çıktıları (publish klasöründe olacak)
❌ obj/                               # Build ara dosyaları
❌ .vs/                               # Visual Studio cache
❌ *.suo
❌ *.user
```

#### 4. Source Code Dosyaları (publish edilmiş DLL'ler yeterli)
```
❌ Controllers/*.cs                   # Source code (DLL'de zaten var)
❌ Models/*.cs                        # Source code (DLL'de zaten var)
❌ Services/*.cs                      # Source code (DLL'de zaten var)
❌ Data/*.cs                          # Source code (DLL'de zaten var)
❌ Program.cs                         # Source code (DLL'de zaten var)
❌ RandevuWeb.csproj                 # Proje dosyası (gerekli değil)
```

#### 5. Hassas Veri Dosyaları
```
❌ Data/*.json                        # JSON veri dosyaları (hassas bilgiler)
❌ Data/*.db                          # SQLite dosyaları (eğer varsa)
❌ appsettings.json.example           # Örnek dosya
```

#### 6. Dokümantasyon Dosyaları
```
❌ *.md                               # Markdown dosyaları (opsiyonel)
❌ README.md
❌ CANLIYA_ALMA_REHBERI.md
❌ PRODUCTION_DEPLOYMENT.md
❌ SQL_MIGRATION.md
❌ backup-and-restore-guide.md
```

---

## 🎯 HIZLI DEPLOYMENT ADIMLARI

### 1. Publish Et
```powershell
cd "C:\Users\Muhammed Göktuğ\Desktop\randevu web"
dotnet publish -c Release -o ./publish
```

### 2. appsettings.json Kontrolü
`publish/appsettings.json` dosyasını kontrol edin ve production connection string'i ile güncelleyin:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\MSSQLSERVER2022;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;"
  }
}
```

### 3. Dosyaları Yükle
`publish/` klasöründeki **TÜM** dosyaları FTP/FileZilla ile hosting'e yükleyin.

### 4. Veritabanını Oluştur
Plesk panelinden veya SQL Server Management Studio ile `database-script.sql` dosyasını çalıştırın.

### 5. Test Et
Uygulamayı tarayıcıda açıp test edin.

---

## 📊 DOSYA BOYUTU TAHMİNİ

- **Publish klasörü:** ~50-100 MB (runtime dahil)
- **Views klasörü:** ~500 KB
- **wwwroot klasörü:** ~1-5 MB
- **Migrations klasörü:** ~50 KB
- **Toplam:** ~50-110 MB

---

## ⚠️ ÖNEMLİ NOTLAR

1. **web.config** dosyası mutlaka yüklenmeli (IIS için gerekli)
2. **appsettings.json** production connection string ile güncellenmeli
3. **Migrations** klasörü mutlaka yüklenmeli (EF Core migration için)
4. **Views** klasörü mutlaka yüklenmeli (Razor view'lar için)
5. **wwwroot** klasörü mutlaka yüklenmeli (CSS, JS dosyaları için)

---

## 🔍 KONTROL LİSTESİ

Deployment öncesi kontrol:
- [ ] `dotnet publish -c Release` komutu çalıştırıldı
- [ ] `publish/appsettings.json` production connection string ile güncellendi
- [ ] `web.config` dosyası mevcut
- [ ] `publish/Views/` klasörü mevcut
- [ ] `publish/wwwroot/` klasörü mevcut
- [ ] `publish/Migrations/` klasörü mevcut
- [ ] `publish/RandevuWeb.dll` dosyası mevcut

Deployment sonrası kontrol:
- [ ] Dosyalar hosting'e yüklendi
- [ ] Veritabanı oluşturuldu (`database-script.sql` çalıştırıldı)
- [ ] Uygulama çalışıyor
- [ ] Veritabanı bağlantısı başarılı
- [ ] Sayfalar açılıyor

---

## 🆘 SORUN GİDERME

**500 Internal Server Error:**
- `appsettings.json` connection string'ini kontrol edin
- `web.config` dosyasının yüklendiğinden emin olun
- IIS log dosyalarını kontrol edin

**Veritabanı bağlantı hatası:**
- Connection string'i kontrol edin
- SQL Server servisinin çalıştığından emin olun
- Kullanıcı adı ve şifrenin doğru olduğundan emin olun

**Sayfa bulunamadı (404):**
- `Views/` klasörünün yüklendiğinden emin olun
- `wwwroot/` klasörünün yüklendiğinden emin olun

