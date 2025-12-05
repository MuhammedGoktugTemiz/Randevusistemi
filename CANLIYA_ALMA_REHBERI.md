# 🚀 Canlıya Alma Rehberi - SQL Script ile

## 📋 Adım Adım Canlıya Alma

### 1. SQL Script'i Hazırlama

`database-script.sql` dosyası hazır! Bu dosya:
- ✅ Veritabanını oluşturur
- ✅ Tüm tabloları oluşturur
- ✅ Foreign key ilişkilerini kurar
- ✅ Index'leri ekler
- ✅ Migration history'yi kaydeder

### 2. SQL Server Management Studio ile Canlıya Alma

**Adım 1: SQL Server Management Studio'yu Açın**
- Production SQL Server'a bağlanın

**Adım 2: SQL Script'i Çalıştırın**
1. `database-script.sql` dosyasını açın
2. Production SQL Server'a bağlı olduğunuzdan emin olun
3. Script'i çalıştırın (F5 veya Execute)

**Adım 3: Kontrol Edin**
Script'in sonundaki kontrol sorguları otomatik çalışacak:
- Tablo sayıları
- Foreign key'ler

### 3. Alternatif: Komut Satırı ile (sqlcmd)

```powershell
sqlcmd -S PRODUCTION_SERVER -d master -i database-script.sql
```

veya belirli bir veritabanına:

```powershell
sqlcmd -S PRODUCTION_SERVER -d randevu_sistemi -i database-script.sql
```

### 4. Uygulama Dosyalarını Canlıya Alın

**Publish Komutu:**
```powershell
dotnet publish -c Release -o ./publish
```

**Canlıya Alınması Gerekenler:**
- `publish/` klasöründeki tüm dosyalar
- `appsettings.json` (production ayarları ile)
- `wwwroot/` klasörü
- `Views/` klasörü

**Canlıya Alınmaması Gerekenler:**
- `bin/`, `obj/` klasörleri
- `*.ps1` script dosyaları
- `Migrations/` klasörü (opsiyonel)
- Development dosyaları

### 5. Production appsettings.json Ayarları

Production sunucuda `appsettings.json` dosyasını şu şekilde oluşturun:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=PRODUCTION_SERVER;Database=randevu_sistemi;User Id=PROD_USER;Password=SECURE_PASSWORD;TrustServerCertificate=False;Connection Timeout=30;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "DefaultUser": {
    "Username": "admin",
    "Password": "SECURE_PASSWORD_HASH"
  },
  "WhatsApp": {
    "AccessToken": "PRODUCTION_ACCESS_TOKEN",
    "PhoneNumberId": "PRODUCTION_PHONE_NUMBER_ID"
  }
}
```

### 6. Veri Aktarımı (Eğer Mevcut Veriler Varsa)

**JSON Dosyalarından Veri Aktarımı:**

1. `Data/` klasöründeki JSON dosyalarını production sunucuya kopyalayın
2. Uygulamayı bir kez çalıştırın
3. `Program.cs` otomatik olarak JSON verilerini SQL'e aktaracak

**Veya Manuel SQL ile:**

```sql
-- Örnek: Doktor ekleme
INSERT INTO Doctors (Name, Specialty, Username, Password, PhoneNumber)
VALUES ('Dr. Ahmet Yılmaz', 'Ortodonti', 'ahmet', '$2a$11$...', '05551234567');
```

### 7. Kontrol Listesi

- [ ] SQL script çalıştırıldı
- [ ] Veritabanı oluşturuldu
- [ ] Tablolar oluşturuldu
- [ ] Foreign key'ler kuruldu
- [ ] Uygulama dosyaları kopyalandı
- [ ] `appsettings.json` production ayarları ile oluşturuldu
- [ ] Uygulama çalıştırıldı ve test edildi
- [ ] Veriler aktarıldı (varsa)

### 8. Sorun Giderme

**SQL Script Çalışmıyorsa:**
- SQL Server versiyonunu kontrol edin (SQL Server 2019+)
- Kullanıcı yetkilerini kontrol edin
- Connection string'i kontrol edin

**Uygulama Bağlanamıyorsa:**
- `appsettings.json` connection string'ini kontrol edin
- SQL Server servisinin çalıştığından emin olun
- Firewall ayarlarını kontrol edin

---

## 📝 Özet

1. **SQL Script:** `database-script.sql` dosyasını production SQL Server'da çalıştırın
2. **Uygulama:** `dotnet publish` ile publish edin ve production sunucuya kopyalayın
3. **Ayarlar:** Production `appsettings.json` oluşturun
4. **Test:** Uygulamayı çalıştırıp test edin

**Hazır!** 🎉

