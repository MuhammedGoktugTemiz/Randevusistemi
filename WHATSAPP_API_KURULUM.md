# WhatsApp Business API Kurulum Rehberi

Bu rehber, WhatsApp Business API bilgilerinizi nasıl alacağınızı adım adım açıklar.

## Adım 1: Meta for Developers Hesabı Oluşturma

1. **Meta for Developers'a gidin:**
   - https://developers.facebook.com/ adresine gidin
   - "Get Started" veya "Başlayın" butonuna tıklayın

2. **Facebook hesabınızla giriş yapın:**
   - Mevcut Facebook hesabınızla giriş yapın
   - Eğer hesabınız yoksa, önce bir Facebook hesabı oluşturun

3. **Geliştirici hesabı oluşturun:**
   - Gerekli bilgileri doldurun (ad, soyad, e-posta)
   - Telefon numaranızı doğrulayın
   - Geliştirici hesabınızı onaylayın

## Adım 2: WhatsApp Business API Uygulaması Oluşturma

1. **Uygulama Oluştur:**
   - Meta for Developers ana sayfasında "My Apps" (Uygulamalarım) bölümüne gidin
   - "Create App" (Uygulama Oluştur) butonuna tıklayın

2. **Uygulama Tipi Seçin:**
   - "Business" (İş) seçeneğini seçin
   - "Next" (İleri) butonuna tıklayın

3. **Uygulama Bilgilerini Doldurun:**
   - **App Name (Uygulama Adı):** Örn: "Randevu Sistemi WhatsApp"
   - **App Contact Email:** İletişim e-postanız
   - **Business Account:** İş hesabınızı seçin (yoksa oluşturun)
   - "Create App" butonuna tıklayın

## Adım 3: WhatsApp Ürününü Ekleme

1. **WhatsApp Ürününü Ekle:**
   - Uygulama paneline geldiğinizde, sol menüden "WhatsApp" ürününü bulun
   - "Set up" (Kurulum) veya "Get Started" butonuna tıklayın

2. **Test Telefon Numarası Ekleme (Test Aşaması):**
   - İlk aşamada test numarası ekleyebilirsiniz
   - "Add phone number" (Telefon numarası ekle) butonuna tıklayın
   - Test için kullanmak istediğiniz numarayı girin (0533 550 60 53)
   - SMS ile doğrulama kodu gönderilir, kodu girin

## Adım 4: Access Token Alma

1. **API Setup Sayfasına Gidin:**
   - Sol menüden "API Setup" (API Kurulumu) seçeneğine tıklayın

2. **Temporary Access Token (Geçici Token):**
   - Sayfada "Temporary access token" bölümünü bulun
   - "Copy" (Kopyala) butonuna tıklayarak token'ı kopyalayın
   - ⚠️ **Not:** Bu token 24 saat geçerlidir, sadece test için kullanılır

3. **Kalıcı Access Token Oluşturma:**
   - Sol menüden "WhatsApp" > "API Setup" bölümüne gidin
   - "Access Tokens" bölümünde "Generate Token" (Token Oluştur) butonuna tıklayın
   - İş hesabınızı seçin
   - Token'ı kopyalayın ve güvenli bir yere kaydedin
   - ⚠️ **Önemli:** Bu token'ı kimseyle paylaşmayın!

## Adım 5: Phone Number ID Alma

1. **Phone Number ID'yi Bulun:**
   - Sol menüden "WhatsApp" > "API Setup" bölümüne gidin
   - "Phone number ID" bölümünü bulun
   - Bu ID genellikle şu formatta olur: `123456789012345`
   - "Copy" (Kopyala) butonuna tıklayarak ID'yi kopyalayın

2. **Alternatif Yol:**
   - Eğer Phone Number ID görünmüyorsa:
   - Sol menüden "WhatsApp" > "Phone Numbers" bölümüne gidin
   - Telefon numaranızın yanındaki "Show" (Göster) butonuna tıklayın
   - Phone Number ID'yi görebilirsiniz

## Adım 6: appsettings.json Dosyasını Güncelleme

1. **Projenizde `appsettings.json` dosyasını açın**

2. **WhatsApp bölümünü güncelleyin:**
   ```json
   {
     "WhatsApp": {
       "AccessToken": "BURAYA_ACCESS_TOKEN_YAPIŞTIRIN",
       "PhoneNumberId": "BURAYA_PHONE_NUMBER_ID_YAPIŞTIRIN"
     }
   }
   ```

3. **Örnek:**
   ```json
   {
     "WhatsApp": {
       "AccessToken": "EAABwzLixnjYBO7ZCZBZAZB...",
       "PhoneNumberId": "123456789012345"
     }
   }
   ```

## Adım 7: Test Mesajı Gönderme

1. **Test için bir doktor numarası ekleyin:**
   - Doktor düzenleme sayfasından test numarasını ekleyin (0539 508 3860 → 905395083860)

2. **Randevu oluşturun:**
   - Yeni bir randevu oluşturun
   - Sistem otomatik olarak WhatsApp mesajı gönderecektir

3. **Logları kontrol edin:**
   - Uygulama loglarında mesaj gönderim durumunu görebilirsiniz
   - Başarılı mesajlar için: "WhatsApp mesajı başarıyla gönderildi"
   - Hatalar için: Hata mesajları loglarda görünecektir

## Önemli Notlar

### Test vs Production

**Test Aşaması:**
- İlk 24 saat için geçici token kullanabilirsiniz
- Test numarasına sadece kayıtlı numaralara mesaj gönderebilirsiniz
- Günlük mesaj limiti vardır (genellikle 1000 mesaj)

**Production Aşaması:**
- Meta Business Verification (İş Doğrulaması) yapmanız gerekir
- Kalıcı Access Token oluşturmanız gerekir
- Ücretli bir servistir (mesaj başına ücret)

### Mesaj Gönderme Limitleri

- **Test Aşaması:** Günlük 1000 mesaj limiti
- **Production:** Mesaj başına ücret (ülkeye göre değişir)
- **Rate Limiting:** Saniyede belirli sayıda mesaj gönderebilirsiniz

### Telefon Numarası Formatı

- WhatsApp API uluslararası format bekler
- Türkiye için: `90` + telefon numarası (başındaki 0 olmadan)
- Örnek: `0533 550 60 53` → `905335506053`
- Sistem otomatik olarak formatlar, ancak doğru formatta girmek daha iyidir

## Sorun Giderme

### "Invalid OAuth access token" Hatası
- Token'ın süresi dolmuş olabilir, yeni token oluşturun
- Token'ı doğru kopyaladığınızdan emin olun

### "Phone number not registered" Hatası
- Alıcı numaranın WhatsApp'ta kayıtlı olduğundan emin olun
- Test aşamasında sadece kayıtlı numaralara mesaj gönderebilirsiniz

### "Rate limit exceeded" Hatası
- Çok fazla mesaj gönderiyorsunuz
- Birkaç dakika bekleyip tekrar deneyin

### Mesajlar Gönderilmiyor
1. `appsettings.json` dosyasını kontrol edin
2. Access Token ve Phone Number ID'nin doğru olduğundan emin olun
3. Uygulama loglarını kontrol edin
4. Meta for Developers konsolunda API kullanımını kontrol edin

## Ek Kaynaklar

- **Meta for Developers Dokümantasyonu:**
  https://developers.facebook.com/docs/whatsapp

- **WhatsApp Business API Dokümantasyonu:**
  https://developers.facebook.com/docs/whatsapp/cloud-api

- **API Referansı:**
  https://developers.facebook.com/docs/whatsapp/cloud-api/reference

## Destek

Sorun yaşarsanız:
1. Meta for Developers konsolundaki hata mesajlarını kontrol edin
2. Uygulama loglarını inceleyin
3. Meta for Developers dokümantasyonunu okuyun

