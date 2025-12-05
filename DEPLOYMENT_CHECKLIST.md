# ✅ Deployment Kontrol Listesi

Bu kontrol listesini takip ederek deployment'ı adım adım yapabilirsiniz.

## 📋 ÖN HAZIRLIK

- [ ] Visual Studio 2019 veya üzeri yüklü
- [ ] .NET Framework 4.8 SDK yüklü
- [ ] SQL Server yüklü ve çalışıyor
- [ ] Hosting bilgileri hazır (SQL Server adresi, kullanıcı adı, şifre)

## 🔨 BUILD İŞLEMİ

### Otomatik Build (Önerilen)
- [ ] PowerShell'i yönetici olarak açın
- [ ] Proje klasörüne gidin: `cd "C:\Users\Muhammed Göktuğ\Desktop\randevu web"`
- [ ] Build script'ini çalıştırın: `.\BUILD_AND_DEPLOY.ps1`
- [ ] Build başarılı oldu mu? (Hata yoksa devam edin)

### Manuel Build (Alternatif)
- [ ] Visual Studio'yu açın
- [ ] `randevu web.sln` dosyasını açın
- [ ] Solution Configuration → **Release** seçin
- [ ] Build → **Rebuild Solution**
- [ ] Build başarılı oldu mu? (Hata yoksa devam edin)

## ✅ DEPLOYMENT KONTROLÜ

- [ ] `CHECK_DEPLOYMENT.ps1` script'ini çalıştırın: `.\CHECK_DEPLOYMENT.ps1`
- [ ] Tüm kontroller başarılı mı? (Hata yoksa devam edin)
- [ ] `deploy/` klasörü oluşturuldu mu?

## 📦 DOSYA HAZIRLIĞI

### web.config Güncelleme
- [ ] `deploy/web.config` dosyasını açın
- [ ] Connection string'i hosting'e göre güncelleyin:
  ```xml
  <add name="DefaultConnection" 
       connectionString="Server=SQL_SERVER_ADRESI;Database=dtomeral_randevu_sistemi;User Id=dtomeral_randevu4;Password=13579Mami.*;TrustServerCertificate=True;Connection Timeout=30;MultipleActiveResultSets=True;" 
       providerName="System.Data.SqlClient" />
  ```
- [ ] `debug="false"` olduğundan emin olun
- [ ] `customErrors mode="RemoteOnly"` olduğundan emin olun

### Dosya Kontrolü
- [ ] `deploy/bin/` klasöründe DLL'ler var mı?
- [ ] `deploy/Views/` klasörü var mı?
- [ ] `deploy/wwwroot/` klasörü var mı?
- [ ] `deploy/App_Start/` klasörü var mı?
- [ ] `deploy/web.config` dosyası var mı?
- [ ] `deploy/Global.asax` dosyası var mı?

## 🚀 HOSTING'E YÜKLEME

### FTP ile Yükleme
- [ ] FTP bilgilerinizi alın (Plesk panelinden)
- [ ] FileZilla veya WinSCP ile bağlanın
- [ ] `deploy/` klasöründeki TÜM dosyaları hosting'e yükleyin:
  - [ ] `bin/` klasöründeki TÜM dosyalar → hosting/bin/
  - [ ] `Views/` klasörü → hosting/Views/
  - [ ] `wwwroot/` klasörü → hosting/wwwroot/
  - [ ] `App_Start/` klasörü → hosting/App_Start/
  - [ ] `web.config` → hosting kök dizini
  - [ ] `Global.asax` → hosting kök dizini

### Plesk File Manager ile Yükleme
- [ ] Plesk panelinde **File Manager**'ı açın
- [ ] `deploy/` klasöründeki dosyaları tek tek veya klasör olarak yükleyin

## ⚙️ IIS AYARLARI

- [ ] Plesk panelinde **Application Pool** seçin
- [ ] **.NET CLR Version** → **v4.0** seçin
- [ ] **Managed Pipeline Mode** → **Integrated** seçin
- [ ] **Start Mode** → **AlwaysRunning** (opsiyonel)
- [ ] Application Pool'un doğru klasöre işaret ettiğinden emin olun

## 🗄️ VERİTABANI

- [ ] Plesk panelinden **Databases** → **SQL Server** bölümüne gidin
- [ ] `database-script.sql` dosyasını çalıştırın
- [ ] Veya SQL Server Management Studio ile bağlanıp script'i çalıştırın
- [ ] Veritabanı başarıyla oluşturuldu mu?

## 🧪 TEST

- [ ] Tarayıcıda `randevu.dtomeralbayrak.com` adresini açın
- [ ] Giriş sayfası görünüyor mu?
- [ ] Admin girişi yapın: `admin` / `Admin123.*`
- [ ] Giriş başarılı mı?
- [ ] Dashboard açılıyor mu?
- [ ] Sayfalar çalışıyor mu?

## 🆘 SORUN GİDERME

### HTTP 500 Hatası
- [ ] IIS log dosyalarını kontrol ettiniz mi? (Plesk → Logs)
- [ ] `bin/` klasöründe tüm DLL'ler yüklü mü?
- [ ] Connection string doğru mu?
- [ ] IIS Application Pool .NET Framework 4.8'e ayarlı mı?

### Veritabanı Bağlantı Hatası
- [ ] Connection string'i kontrol ettiniz mi?
- [ ] SQL Server çalışıyor mu?
- [ ] Kullanıcı adı ve şifre doğru mu?
- [ ] Firewall ayarları doğru mu?

### Sayfa Bulunamadı (404)
- [ ] `Views/` klasörü yüklü mü?
- [ ] `wwwroot/` klasörü yüklü mü?
- [ ] Route yapılandırması doğru mu?

## ✅ DEPLOYMENT TAMAMLANDI

- [ ] Tüm adımlar tamamlandı
- [ ] Uygulama çalışıyor
- [ ] Testler başarılı
- [ ] Production'a hazır

---

## 📞 DESTEK

Sorun yaşarsanız:
1. `PRODUCTION_DEPLOYMENT_FINAL.md` dosyasını okuyun
2. IIS log dosyalarını kontrol edin
3. `web.config` dosyasını kontrol edin
4. Hosting desteğine başvurun

