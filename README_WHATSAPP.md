# WhatsApp Business API Entegrasyonu

Bu sistem, WhatsApp Business API kullanarak doktorlara otomatik randevu bildirimleri ve hatırlatmaları gönderir.

## Kurulum

### 1. WhatsApp Business API Bilgilerini Alın

1. [Meta for Developers](https://developers.facebook.com/) hesabı oluşturun
2. WhatsApp Business API uygulaması oluşturun
3. Aşağıdaki bilgileri alın:
   - **Access Token**: WhatsApp API erişim token'ı
   - **Phone Number ID**: WhatsApp Business telefon numarası ID'si

### 2. appsettings.json Yapılandırması

`appsettings.json` dosyasına WhatsApp API bilgilerinizi ekleyin:

```json
{
  "WhatsApp": {
    "AccessToken": "YOUR_ACCESS_TOKEN_HERE",
    "PhoneNumberId": "YOUR_PHONE_NUMBER_ID_HERE"
  }
}
```

### 3. Doktor Telefon Numaralarını Ekleyin

1. Doktor listesine gidin
2. Her doktoru düzenleyin
3. "WhatsApp Telefon Numarası" alanına telefon numarasını girin
4. Format: `905551234567` (90 ile başlayan, sadece rakamlar)

## Özellikler

### 1. Randevu Oluşturulduğunda Mesaj

Yeni bir randevu oluşturulduğunda, ilgili doktora otomatik olarak WhatsApp mesajı gönderilir:

```
Merhaba [Doktor Soyadı],

7 Aralık 2024 günü 14:00 saatinde [Hasta Adı] adlı hastanın randevusu tarafınıza oluşturulmuştur.

Hasta: [Hasta Adı]
Tarih: 7 Aralık 2024
Saat: 14:00
Süre: 60 dakika
İşlem: [İşlem Türü]
Diş Numaraları: 11, 12, 13

İyi çalışmalar dileriz.
```

### 2. Randevu Hatırlatması (1 Gün Önce)

Sistem her saat başı kontrol eder ve yarınki randevular için doktorlara hatırlatma mesajı gönderir:

```
Hatırlatma: [Doktor Soyadı],

Yarın (7 Aralık 2024) saat 14:00'de [Hasta Adı] hastasına ait işleminiz bulunmaktadır.

Hasta: [Hasta Adı]
Tarih: 7 Aralık 2024
Saat: 14:00
Süre: 60 dakika
İşlem: [İşlem Türü]
Diş Numaraları: 11, 12, 13

Lütfen randevunuzu unutmayın.
```

## Teknik Detaylar

### Servisler

- **IWhatsAppService**: WhatsApp mesaj gönderme interface'i
- **WhatsAppService**: WhatsApp Business Cloud API implementasyonu
- **WhatsAppReminderService**: Arka planda çalışan hatırlatma servisi

### Background Service

`WhatsAppReminderService` her saat başı çalışır ve:
- Yarınki randevuları kontrol eder
- Henüz hatırlatma gönderilmemiş randevular için mesaj gönderir
- Gönderilen hatırlatmaları `Data/reminders.json` dosyasında saklar

### Mesaj Formatı

Mesajlar Türkçe tarih formatı kullanır ve şu bilgileri içerir:
- Doktor adı (soyadı)
- Hasta adı
- Randevu tarihi ve saati
- Süre
- İşlem türü (varsa)
- Diş numaraları (varsa)

## Sorun Giderme

### Mesajlar Gönderilmiyor

1. `appsettings.json` dosyasında API bilgilerinin doğru olduğundan emin olun
2. Doktor telefon numaralarının doğru formatta olduğundan emin olun
3. Uygulama loglarını kontrol edin
4. WhatsApp Business API hesabınızın aktif olduğundan emin olun

### Hatırlatmalar Çalışmıyor

1. Background service'in çalıştığından emin olun
2. `Data/reminders.json` dosyasının yazılabilir olduğundan emin olun
3. Uygulama loglarını kontrol edin

## Notlar

- WhatsApp Business API ücretli bir servistir
- Mesaj göndermek için alıcının numarası WhatsApp'ta kayıtlı olmalıdır
- Test ortamında sınırlı sayıda mesaj gönderebilirsiniz
- Production'da rate limiting'e dikkat edin

