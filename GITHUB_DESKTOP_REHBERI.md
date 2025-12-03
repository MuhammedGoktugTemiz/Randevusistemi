# 🖥️ GitHub Desktop ile Yükleme Rehberi

Bu rehber, GitHub Desktop uygulaması kullanarak projenizi GitHub'a yüklemeniz için adım adım talimatlar içerir.

## ⚠️ ÖNEMLİ: Commit Edilmemesi Gerekenler

Aşağıdaki dosyalar **ASLA** commit edilmemelidir:
- ❌ `appsettings.json` (şifreler içerir)
- ❌ `Data/` klasörü (hasta ve doktor bilgileri içerir)
- ❌ `bin/` ve `obj/` klasörleri (derleme dosyaları)
- ❌ `.vs/` klasörü (Visual Studio ayarları)

Bu dosyalar `.gitignore` dosyasına eklenmiştir ve GitHub Desktop otomatik olarak ignore edecektir.

## 📋 Adım Adım Talimatlar

### 1. GitHub Desktop'ta Repository Oluşturma

1. **GitHub Desktop** uygulamasını açın
2. Sol üstteki **"File"** menüsüne tıklayın
3. **"Add"** → **"Create New Repository"** seçin
4. Şu bilgileri doldurun:
   - **Name:** `randevuweb` (veya istediğiniz isim)
   - **Description:** "Randevu Web Sistemi" (opsiyonel)
   - **Local Path:** Projenizin bulunduğu klasörü seçin
     - Örnek: `C:\Users\Muhammed Göktuğ\Desktop\randevu web`
   - **Git ignore:** `.gitignore` dosyası zaten var, "None" seçin
   - **License:** İstediğiniz lisansı seçin (opsiyonel)
5. **"Create Repository"** butonuna tıklayın

### 2. GitHub'da Remote Repository Oluşturma

1. GitHub Desktop'ta sağ üstteki **"Publish repository"** butonuna tıklayın
2. Şu ayarları yapın:
   - **Name:** `randevuweb` (veya istediğiniz isim)
   - **Description:** "Randevu Web Sistemi" (opsiyonel)
   - **Keep this code private:** ✅ **İŞARETLEYİN** (güvenlik için önemli!)
   - **Organization:** (boş bırakın, kendi hesabınıza yükleyeceksiniz)
3. **"Publish Repository"** butonuna tıklayın

### 3. Commit Edilecek Dosyaları Kontrol Etme

GitHub Desktop'ta sol panelde **"Changes"** sekmesine gidin ve şunları kontrol edin:

#### ✅ Commit Edilmesi GEREKEN Dosyalar:
- ✅ Tüm `.cs` dosyaları (Controllers, Models, Services, vb.)
- ✅ Tüm `.cshtml` dosyaları (Views)
- ✅ `.csproj` dosyası
- ✅ `Program.cs`
- ✅ `README.md`
- ✅ `.gitignore`
- ✅ `.gitattributes`
- ✅ `appsettings.json.example`
- ✅ Tüm `.md` dosyaları (rehberler)
- ✅ `wwwroot/` klasörü (CSS, JS dosyaları)

#### ❌ Commit Edilmemesi GEREKEN Dosyalar (Görünmemeli):
- ❌ `appsettings.json` (GÖRÜNMEMELİ!)
- ❌ `Data/` klasörü (GÖRÜNMEMELİ!)
- ❌ `bin/` klasörü (GÖRÜNMEMELİ!)
- ❌ `obj/` klasörü (GÖRÜNMEMELİ!)

### 4. Eğer Hassas Dosyalar Görünüyorsa

Eğer `appsettings.json` veya `Data/` klasörü "Changes" listesinde görünüyorsa:

1. GitHub Desktop'ta sağ tıklayın
2. **"Ignore"** seçeneğini seçin
3. VEYA manuel olarak `.gitignore` dosyasını kontrol edin

### 5. İlk Commit Yapma

1. GitHub Desktop'ta sol alttaki **"Summary"** alanına commit mesajı yazın:
   ```
   İlk commit: Randevu Web Sistemi - Güvenlik özellikleri eklendi
   ```
2. **"Commit to main"** butonuna tıklayın

### 6. GitHub'a Push Etme

1. Commit yaptıktan sonra üstteki **"Push origin"** butonuna tıklayın
2. GitHub Desktop projenizi GitHub'a yükleyecektir

### 7. Kontrol Etme

1. GitHub.com'a gidin
2. Repository'nize gidin
3. Şunları kontrol edin:
   - ✅ `appsettings.json` dosyası YOK
   - ✅ `Data/` klasörü YOK
   - ✅ `bin/` ve `obj/` klasörleri YOK
   - ✅ Kaynak kod dosyaları VAR

## 🔍 GitHub Desktop'ta Hassas Dosya Kontrolü

### Manuel Kontrol:

1. GitHub Desktop'ta **"Repository"** → **"Repository Settings"** → **"Ignored Files"** sekmesine gidin
2. Şu dosyaların listede olduğundan emin olun:
   - `appsettings.json`
   - `Data/`
   - `bin/`
   - `obj/`

### Eğer Hassas Dosyalar Commit Edildiyse:

1. GitHub Desktop'ta **"History"** sekmesine gidin
2. Son commit'e sağ tıklayın
3. **"Revert this commit"** seçin
4. VEYA GitHub web sitesinden dosyaları silin ve yeni commit yapın

## 🚨 Güvenlik Kontrol Listesi

Commit etmeden önce:

- [ ] `appsettings.json` "Changes" listesinde YOK
- [ ] `Data/` klasörü "Changes" listesinde YOK
- [ ] `bin/` ve `obj/` klasörleri "Changes" listesinde YOK
- [ ] Repository **Private** olarak ayarlandı
- [ ] `.gitignore` dosyası commit ediliyor
- [ ] `appsettings.json.example` dosyası commit ediliyor

## 📝 Sonraki Adımlar

1. Repository'yi **Private** olarak ayarlayın (Settings → Danger Zone → Change visibility)
2. Collaborators ekleyin (Settings → Collaborators)
3. Branch protection kuralları ekleyebilirsiniz (opsiyonel)

## ✅ Başarı!

Projeniz artık GitHub'da güvenli bir şekilde saklanıyor!

## 🆘 Sorun Giderme

### "appsettings.json görünüyor" Sorunu:

1. GitHub Desktop'ta dosyaya sağ tıklayın
2. **"Ignore"** seçin
3. `.gitignore` dosyasına `appsettings.json` satırının eklendiğinden emin olun

### "Data klasörü görünüyor" Sorunu:

1. GitHub Desktop'ta klasöre sağ tıklayın
2. **"Ignore"** seçin
3. `.gitignore` dosyasına `Data/` satırının eklendiğinden emin olun

### "Repository zaten var" Hatası:

GitHub Desktop'ta:
1. **"File"** → **"Add"** → **"Add Existing Repository"**
2. Proje klasörünüzü seçin
3. GitHub'da repository'yi manuel olarak oluşturun
4. **"Repository"** → **"Repository Settings"** → **"Remote"** sekmesinde GitHub URL'ini ekleyin

