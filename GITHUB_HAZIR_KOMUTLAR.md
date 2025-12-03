# 🚀 GitHub'a Yükleme - Hazır Komutlar

Bu dosya, projenizi GitHub'a yüklemek için gereken tüm komutları içerir. **Sırayla çalıştırın.**

## ⚠️ ÖNEMLİ: Önce Git Kurulumu

Eğer Git yüklü değilse:
1. https://git-scm.com/download/win adresinden Git'i indirin
2. Kurulumu tamamlayın
3. Terminal'i yeniden başlatın

## 📋 Adım Adım Komutlar

### 1. Git Repository Başlatma

```bash
git init
```

### 2. Hassas Dosyaları Git'ten Kaldırma (Güvenlik)

Eğer daha önce Git'e eklenmişse:

```bash
git rm --cached appsettings.json
git rm -r --cached Data/
git rm -r --cached bin/
git rm -r --cached obj/
```

### 3. Tüm Dosyaları Staging Area'ya Ekleme

```bash
git add .
```

### 4. İlk Commit

```bash
git commit -m "İlk commit: Randevu Web Sistemi - Güvenlik özellikleri eklendi"
```

### 5. GitHub Repository Oluşturma

**Manuel olarak yapmanız gereken:**
1. https://github.com adresine gidin
2. Sağ üstteki "+" butonuna tıklayın
3. "New repository" seçin
4. Repository adını girin (örn: `randevu-web`)
5. **Private** seçin (önerilen)
6. "Initialize with README" seçeneğini **İŞARETLEMEYİN**
7. "Create repository" butonuna tıklayın

### 6. GitHub Remote Ekleme

**BURAYA_KULLANICI_ADINIZI ve REPO_ADINIZI yazın:**

```bash
git remote add origin https://github.com/BURAYA_KULLANICI_ADI/BURAYA_REPO_ADI.git
```

Örnek:
```bash
git remote add origin https://github.com/muhammedgoktug/randevu-web.git
```

### 7. Ana Branch'i Main Olarak Ayarlama

```bash
git branch -M main
```

### 8. GitHub'a Push Etme

```bash
git push -u origin main
```

Bu komut sizden GitHub kullanıcı adı ve şifre isteyebilir. Eğer 2FA aktifse, Personal Access Token kullanmanız gerekebilir.

## 🔍 Kontrol Komutları

### Git Durumunu Kontrol Etme

```bash
git status
```

### Staged Dosyaları Görme

```bash
git diff --cached --name-only
```

### Hassas Dosyaların Git'te Olmadığını Kontrol Etme

```bash
git ls-files | findstr "appsettings.json"
git ls-files | findstr "Data"
```

Eğer hiçbir sonuç çıkmazsa, hassas dosyalar Git'e eklenmemiş demektir. ✅

## 🚨 Sorun Giderme

### "Git yüklü değil" Hatası

Git'i yükleyin: https://git-scm.com/download/win

### "Authentication failed" Hatası

GitHub'da Personal Access Token oluşturun:
1. GitHub → Settings → Developer settings → Personal access tokens → Tokens (classic)
2. "Generate new token" tıklayın
3. İzinleri seçin (repo)
4. Token'ı kopyalayın
5. Şifre yerine token'ı kullanın

### "Remote origin already exists" Hatası

```bash
git remote remove origin
git remote add origin https://github.com/KULLANICI_ADI/REPO_ADI.git
```

### Hassas Dosyalar Git'e Eklenmişse

```bash
# Dosyaları Git'ten kaldır (dosya silinmez)
git rm --cached appsettings.json
git rm -r --cached Data/

# Değişiklikleri commit et
git commit -m "Hassas dosyaları Git'ten kaldır"

# GitHub'a push et
git push origin main
```

## ✅ Başarı Kontrolü

GitHub repository'nize gidin ve şunları kontrol edin:

- ✅ `appsettings.json` dosyası YOK
- ✅ `Data/` klasörü YOK
- ✅ `bin/` ve `obj/` klasörleri YOK
- ✅ Kaynak kod dosyaları VAR (.cs, .cshtml, vb.)
- ✅ `README.md` dosyası VAR
- ✅ `.gitignore` dosyası VAR

## 📝 Sonraki Adımlar

1. Repository'yi **Private** olarak ayarlayın (Settings → Change visibility)
2. Collaborators ekleyin (Settings → Collaborators)
3. GitHub Actions ile CI/CD kurulumu yapabilirsiniz (opsiyonel)

## 🎉 Tamamlandı!

Projeniz artık GitHub'da güvenli bir şekilde saklanıyor!

