# 💾 Veritabani Backup ve Restore Rehberi

## 📋 Durum: Backup Dosyası Boş

Eğer backup dosyası boşsa, önce mevcut veritabanından backup oluşturmanız gerekiyor.

---

## 🔄 ADIM 1: Mevcut Veritabanından Backup Oluşturma

### Yöntem 1: SQL Script ile (Önerilen)

**1. `create-backup.sql` dosyasını açın**

**2. Backup klasörünü oluşturun:**
```powershell
# PowerShell'de
New-Item -ItemType Directory -Path "C:\Backup" -Force
```

**3. SQL Server Management Studio'da çalıştırın:**
- `create-backup.sql` dosyasını açın
- Backup path'i kendi klasörünüze göre düzenleyin
- Script'i çalıştırın (F5)

### Yöntem 2: SQL Server Management Studio GUI ile

**1. SQL Server Management Studio'yu açın**

**2. Veritabanına sağ tıklayın:**
- `randevu_sistemi` → `Tasks` → `Back Up...`

**3. Backup ayarlarını yapın:**
- **Backup type:** Full
- **Backup component:** Database
- **Destination:** Add → Backup dosyası yolunu seçin (örn: `C:\Backup\randevu_sistemi.bak`)
- **Options:** Compression seçeneğini işaretleyin

**4. OK'a tıklayın**

---

## 📥 ADIM 2: Backup Dosyasını Canlıya Taşıma

**1. Backup dosyasını kopyalayın:**
- Local: `C:\Backup\randevu_sistemi.bak`
- Production: `\\PRODUCTION_SERVER\Backup\randevu_sistemi.bak` veya FTP ile

**2. Production SQL Server'a bağlanın**

**3. `restore-backup.sql` dosyasını çalıştırın:**
- Backup dosya yolunu production sunucudaki yola göre düzenleyin
- Script'i çalıştırın

---

## 🚀 ADIM 3: Alternatif - Doğrudan SQL Script ile Canlıya Alma

Eğer backup dosyası oluşturmak istemiyorsanız, doğrudan SQL script ile canlıya alabilirsiniz:

**1. `database-script.sql` dosyasını kullanın**

**2. Production SQL Server'da çalıştırın:**
- Bu script veritabanını ve tabloları oluşturur
- Foreign key'leri kurar
- Index'leri ekler

**3. Verileri aktarın:**
- JSON dosyalarından otomatik aktarım (uygulama çalıştığında)
- Veya manuel SQL INSERT komutları ile

---

## 📝 Detaylı Adımlar

### Backup Oluşturma (Detaylı)

```sql
-- 1. Backup klasörünü oluşturun (Windows Explorer'dan veya PowerShell'den)
-- C:\Backup klasörü

-- 2. SQL Server Management Studio'da çalıştırın
USE master;
GO

BACKUP DATABASE [randevu_sistemi]
TO DISK = 'C:\Backup\randevu_sistemi.bak'
WITH FORMAT,
     NAME = 'Randevu Sistemi Full Backup',
     DESCRIPTION = 'Randevu Sistemi Veritabani Backup',
     COMPRESSION,
     STATS = 10;
GO
```

### Restore (Detaylı)

```sql
-- 1. Production SQL Server'a bağlanın

-- 2. Mevcut bağlantıları kes
USE master;
GO

ALTER DATABASE [randevu_sistemi]
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

-- 3. Restore et
RESTORE DATABASE [randevu_sistemi]
FROM DISK = 'C:\Backup\randevu_sistemi.bak'
WITH REPLACE,
     STATS = 10;
GO

-- 4. Çoklu kullanıcı moduna geri dön
ALTER DATABASE [randevu_sistemi]
SET MULTI_USER;
GO
```

---

## ⚠️ Önemli Notlar

1. **Backup Dosyası Boyutu:** Backup dosyası veritabanı boyutuna göre değişir
2. **Network Transfer:** Büyük backup dosyaları için network hızını kontrol edin
3. **SQL Server Versiyonu:** Production SQL Server versiyonu local'den daha yeni veya aynı olmalı
4. **Kullanıcı Yetkileri:** Backup ve restore için sysadmin veya db_owner yetkisi gerekir

---

## 🔍 Sorun Giderme

### Backup Dosyası Oluşturulamıyor

**Hata:** "The backup file path is invalid"
**Çözüm:** Backup klasörünün var olduğundan ve SQL Server'ın yazma yetkisi olduğundan emin olun

**Hata:** "Access is denied"
**Çözüm:** SQL Server servisinin backup klasörüne yazma yetkisi verin

### Restore Çalışmıyor

**Hata:** "The backup set holds a backup of a database other than the existing database"
**Çözüm:** `WITH REPLACE` parametresini ekleyin

**Hata:** "Exclusive access could not be obtained"
**Çözüm:** `SET SINGLE_USER WITH ROLLBACK IMMEDIATE` kullanın

---

## 📊 Hangi Yöntemi Seçmeliyim?

### ✅ Backup/Restore Yöntemi
**Ne zaman kullanılır:**
- Mevcut veriler varsa
- Veritabanı büyükse
- Hızlı aktarım gerekiyorsa

### ✅ SQL Script Yöntemi
**Ne zaman kullanılır:**
- Yeni veritabanı oluşturuyorsanız
- Veriler JSON dosyalarından aktarılacaksa
- Daha kontrollü kurulum istiyorsanız

---

## 🎯 Önerilen Yol

1. **Local'de backup oluşturun:** `create-backup.sql`
2. **Backup dosyasını production'a kopyalayın**
3. **Production'da restore edin:** `restore-backup.sql`
4. **Uygulamayı production'a deploy edin**

**Hazır!** 🎉

