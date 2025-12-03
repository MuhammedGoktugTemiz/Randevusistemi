# Randevu Sistemi - Web Uygulaması

Diş hekimi randevu yönetim sistemi. ASP.NET Core MVC ile geliştirilmiş, JSON dosyalarında veri saklanıyor.

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

## Teknolojiler

- .NET 8.0
- ASP.NET Core MVC
- JSON dosya tabanlı veri depolama
- Cookie-based authentication

## Kurulum

1. .NET 8.0 SDK'nın yüklü olduğundan emin olun
2. Projeyi klonlayın veya indirin
3. Terminal'de proje klasörüne gidin
4. Projeyi çalıştırın:

```bash
dotnet run
```

5. Tarayıcıda `https://localhost:5001` veya `http://localhost:5000` adresine gidin

## Giriş Bilgileri

Giriş bilgileri `appsettings.json` dosyasında yapılandırılır. İlk kurulum için `appsettings.json.example` dosyasını `appsettings.json` olarak kopyalayın ve bilgileri doldurun.

**Güvenlik:** Şifreler BCrypt ile hash'lenmiştir. Detaylı bilgi için `GUVENLIK_REHBERI.md` dosyasına bakın.

## Veri Depolama

Tüm veriler `Data/` klasöründe JSON dosyalarında saklanır:
- `patients.json` - Hasta verileri
- `doctors.json` - Doktor verileri
- `appointments.json` - Randevu verileri

**ÖNEMLİ:** `Data/` klasörü hassas bilgiler içerir ve Git'e commit edilmemelidir!

## Proje Yapısı

```
RandevuWeb/
├── Controllers/        # MVC Controller'lar
├── Models/            # Veri modelleri
├── Services/          # Veri servisleri
├── Views/             # Razor view'lar
├── wwwroot/           # Statik dosyalar (CSS, JS)
│   ├── css/
│   └── js/
└── Data/              # JSON veri dosyaları (otomatik oluşturulur)
```

## Diş Şeması

Randevu ekleme sayfasında interaktif diş şeması bulunur:
- Üst çene: 18-28
- Alt çene: 48-38, 31-41
- Diş seçimi ile otomatik işlem türü aktifleşir

## Güvenlik Özellikleri

- ✅ BCrypt şifre hash'leme
- ✅ Rate limiting (brute force koruması)
- ✅ Cookie güvenliği (HttpOnly, Secure, SameSite)
- ✅ Git güvenliği (.gitignore yapılandırması)

Detaylı bilgi için `GUVENLIK_REHBERI.md` ve `GUVENLIK_KURULUM.md` dosyalarına bakın.

## Geliştirme

Projeyi geliştirme modunda çalıştırmak için:

```bash
dotnet watch run
```

Bu komut dosya değişikliklerini otomatik algılar ve uygulamayı yeniden başlatır.

## GitHub'a Yükleme

Projeyi GitHub'a yüklemek için `GITHUB_YUKLEME_REHBERI.md` dosyasındaki adımları takip edin.

**ÖNEMLİ:** Hassas dosyalar (`appsettings.json`, `Data/` klasörü) Git'e commit edilmemelidir!

## Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

