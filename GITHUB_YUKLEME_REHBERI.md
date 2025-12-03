# 🚀 GitHub'a Yükleme Rehberi

Bu rehber, projenizi GitHub'a güvenli bir şekilde yüklemeniz için adım adım talimatlar içerir.

## ⚠️ ÖNEMLİ: Hassas Dosyalar

Aşağıdaki dosyalar **ASLA** Git'e commit edilmemelidir:
- ❌ `appsettings.json` (şifreler içerir)
- ❌ `Data/` klasörü (hasta ve doktor bilgileri içerir)
- ❌ `bin/` ve `obj/` klasörleri (derleme dosyaları)
- ❌ `.vs/` klasörü (Visual Studio ayarları)

Bu dosyalar `.gitignore` dosyasına eklenmiştir ve otomatik olarak ignore edilecektir.

## 📋 Adım Adım Kurulum

### 1. Git Kurulumu

Eğer Git yüklü değilse:
1. https://git-scm.com/download/win adresinden Git'i indirin
2. Kurulumu tamamlayın
3. Terminal'i yeniden başlatın

### 2. GitHub'da Repository Oluşturma

1. GitHub.com'a gidin ve giriş yapın
2. Sağ üstteki "+" butonuna tıklayın
3. "New repository" seçeneğini seçin
4. Repository adını girin (örn: `randevu-web`)
5. **Public** veya **Private** seçin (önerilen: Private - hassas bilgiler içerdiği için)
6. **"Initialize this repository with a README"** seçeneğini **İŞARETLEMEYİN**
7. "Create repository" butonuna tıklayın

### 3. Projeyi Git'e Bağlama

Proje klasörünüzde şu komutları çalıştırın:

```bash
# Git repository'sini başlat
git init

# Tüm dosyaları staging area'ya ekle (hassas dosyalar otomatik ignore edilecek)
git add .

# İlk commit'i yap
git commit -m "İlk commit: Randevu Web Sistemi"

# GitHub repository'nizi remote olarak ekleyin
# BURAYA_KENDI_GITHUB_KULLANICI_ADINIZI ve REPOSITORY_ADINIZI yazın
git remote add origin https://github.com/BURAYA_KULLANICI_ADI/REPOSITORY_ADI.git

# Ana branch'i main olarak ayarla
git branch -M main

# GitHub'a push et
git push -u origin main
```

### 4. Hassas Dosyaları Kontrol Etme

Commit etmeden önce şu komutu çalıştırarak hangi dosyaların ekleneceğini kontrol edin:

```bash
git status
```

**ÖNEMLİ:** `appsettings.json` veya `Data/` klasörü listede görünmemeli!

Eğer görünüyorsa:

```bash
# appsettings.json'ı Git'ten kaldır (dosya silinmez, sadece Git tracking'i durur)
git rm --cached appsettings.json

# Data klasörünü Git'ten kaldır
git rm -r --cached Data/

# Değişiklikleri commit et
git commit -m "Hassas dosyaları Git'ten kaldır"
```

### 5. .gitignore Kontrolü

`.gitignore` dosyasının şu satırları içerdiğinden emin olun:

```
Data/
Data/**/*.json
appsettings.json
bin/
obj/
```

## 🔐 Güvenlik Kontrol Listesi

GitHub'a yüklemeden önce:

- [ ] `appsettings.json` Git'e eklenmemiş
- [ ] `Data/` klasörü Git'e eklenmemiş
- [ ] `.gitignore` dosyası doğru yapılandırılmış
- [ ] `appsettings.json.example` dosyası var (örnek konfigürasyon)
- [ ] `README.md` dosyası var ve hassas bilgi içermiyor
- [ ] Repository **Private** olarak ayarlandı (önerilen)

## 📝 README.md Örneği

Projenize bir `README.md` dosyası ekleyin. İşte bir örnek:

```markdown
# Randevu Web Sistemi

Diş hekimi randevu yönetim sistemi.

## Kurulum

1. `appsettings.json.example` dosyasını `appsettings.json` olarak kopyalayın
2. `appsettings.json` dosyasındaki bilgileri doldurun
3. `dotnet restore` komutunu çalıştırın
4. `dotnet run` komutu ile uygulamayı başlatın

## Güvenlik

- Şifreler BCrypt ile hash'lenmiştir
- Rate limiting aktif (brute force koruması)
- Cookie güvenliği aktif

Detaylı bilgi için `GUVENLIK_REHBERI.md` dosyasına bakın.
```

## 🚨 Eğer Hassas Dosyalar Zaten Commit Edildiyse

Eğer `appsettings.json` veya `Data/` klasörü daha önce commit edildiyse:

1. **GitHub'da dosyaları silin** (GitHub web arayüzünden)
2. **Git geçmişini temizleyin:**

```bash
# Son commit'i geri al (dosyalar silinmez)
git reset --soft HEAD~1

# Hassas dosyaları Git'ten kaldır
git rm --cached appsettings.json
git rm -r --cached Data/

# Yeni commit yap
git commit -m "Hassas dosyaları kaldır"

# GitHub'a force push (DİKKATLİ OLUN!)
git push --force origin main
```

**UYARI:** Force push yapmadan önce, GitHub'da dosyaları manuel olarak silin!

## 📞 Yardım

Sorun yaşarsanız:
1. `git status` komutu ile durumu kontrol edin
2. `.gitignore` dosyasını kontrol edin
3. GitHub dokümantasyonuna bakın: https://docs.github.com

## ✅ Tamamlandı!

Projeniz artık GitHub'da güvenli bir şekilde saklanıyor. Hassas bilgiler Git'e eklenmemiş durumda.

