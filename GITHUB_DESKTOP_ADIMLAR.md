# 🖥️ GitHub Desktop - Adım Adım Yükleme

## ⚠️ ÖNEMLİ: Commit Edilmemesi Gerekenler

Bu dosyalar **KESINLIKLE** commit edilmemelidir:
- ❌ `appsettings.json` 
- ❌ `Data/` klasörü
- ❌ `bin/` klasörü
- ❌ `obj/` klasörü

## 📋 Adımlar

### ADIM 1: GitHub Desktop'ta Repository Oluştur

1. **GitHub Desktop** uygulamasını açın
2. **File** → **Add** → **Create New Repository**
3. Şu bilgileri girin:
   ```
   Name: randevuweb
   Local Path: [Projenizin klasörünü seçin]
   Git ignore: None (zaten .gitignore var)
   ```
4. **Create Repository** butonuna tıklayın

### ADIM 2: Hassas Dosyaları Kontrol Et ⚠️

**ÇOK ÖNEMLİ:** GitHub Desktop'ta sol panelde **"Changes"** sekmesine gidin.

**Kontrol Listesi:**
- [ ] `appsettings.json` **GÖRÜNMÜYOR** ✅
- [ ] `Data/` klasörü **GÖRÜNMÜYOR** ✅
- [ ] `bin/` klasörü **GÖRÜNMÜYOR** ✅
- [ ] `obj/` klasörü **GÖRÜNMÜYOR** ✅

**Eğer bu dosyalar görünüyorsa:**
1. Dosyaya/klasöre **sağ tıklayın**
2. **"Ignore"** seçeneğini seçin
3. Tekrar kontrol edin

### ADIM 3: Commit Yap

1. Sol alttaki **"Summary"** alanına şunu yazın:
   ```
   İlk commit: Randevu Web Sistemi - Güvenlik özellikleri eklendi
   ```
2. **"Commit to main"** butonuna tıklayın

### ADIM 4: GitHub'a Yükle

1. Üstteki **"Publish repository"** butonuna tıklayın
2. Şu ayarları yapın:
   ```
   Name: randevuweb
   ✅ Keep this code private (MUTLAKA İŞARETLEYİN!)
   ```
3. **"Publish Repository"** butonuna tıklayın

### ADIM 5: Push Et

1. **"Push origin"** butonuna tıklayın
2. Dosyalar GitHub'a yüklenecek

## ✅ Son Kontrol

GitHub.com'da repository'nize gidin ve kontrol edin:

- ✅ `appsettings.json` **YOK**
- ✅ `Data/` klasörü **YOK**
- ✅ `bin/` ve `obj/` klasörleri **YOK**
- ✅ Kaynak kod dosyaları **VAR**
- ✅ Repository **PRIVATE**

## 🚨 Sorun Giderme

### Hassas Dosyalar Görünüyorsa:

1. GitHub Desktop'ta dosyaya sağ tıklayın
2. **"Ignore"** seçin
3. `.gitignore` dosyasının güncellendiğini kontrol edin

### Repository Zaten Varsa:

1. **File** → **Add** → **Add Existing Repository**
2. Proje klasörünüzü seçin
3. GitHub'da repository URL'ini ekleyin:
   - **Repository** → **Repository Settings** → **Remote**
   - URL: `https://github.com/KULLANICI_ADI/randevuweb.git`

## 📞 Yardım

Detaylı bilgi için `GITHUB_DESKTOP_REHBERI.md` dosyasına bakın.

