# 📊 SQL Server'a Veri Aktarım Yöntemleri

## 🎯 ÖNERİLEN YÖNTEM: EF Core Migration (1. Seçenek)

### ✅ Avantajları:
- Foreign key ilişkileri otomatik kurulur
- Tablo yapıları doğru oluşturulur
- Index'ler otomatik eklenir
- Sonraki migration'lar için hazır olur
- Veritabanı şeması kod ile senkronize kalır

### 📝 Adımlar:

**1. Migration'ları uygula:**
```powershell
dotnet ef database update --project RandevuWeb.csproj
```

**2. Uygulamayı çalıştır (JSON verileri otomatik aktarılır):**
```powershell
dotnet run
```

Uygulama ilk çalıştığında:
- Veritabanı oluşturulur
- Tablolar oluşturulur
- Foreign key ilişkileri kurulur
- JSON dosyalarındaki veriler otomatik aktarılır

---

## 🔄 ALTERNATİF YÖNTEM: Uygulama ile Otomatik (2. Seçenek)

### ✅ Avantajları:
- Tek komutla her şey hazır olur
- Migration ve veri aktarımı birlikte yapılır

### 📝 Adımlar:

**Sadece uygulamayı çalıştır:**
```powershell
dotnet run
```

Program.cs otomatik olarak:
1. Veritabanı bağlantısını test eder
2. Migration'ları uygular (varsa)
3. Veritabanını oluşturur (yoksa)
4. JSON dosyalarındaki verileri aktarır

---

## 🛠️ MANUEL YÖNTEM: SQL Script (3. Seçenek)

### ⚠️ Dikkat:
- Foreign key ilişkilerini manuel kurmanız gerekir
- Tablo yapılarını manuel oluşturmanız gerekir
- Önerilmez, sadece özel durumlar için

### 📝 Adımlar:

**1. Migration'dan SQL script oluştur:**
```powershell
dotnet ef migrations script --project RandevuWeb.csproj -o migration.sql
```

**2. SQL Server Management Studio'da çalıştır:**
- `migration.sql` dosyasını açın
- SQL Server'a bağlanın
- Script'i çalıştırın

---

## 📋 Hangi Yöntemi Seçmeliyim?

### ✅ **1. Seçenek (EF Core Migration)** - ÖNERİLEN
**Ne zaman kullanılır:**
- İlk kez veritabanı oluşturuyorsanız
- Production ortamına deploy ediyorsanız
- Foreign key ilişkilerinin doğru kurulmasını istiyorsanız

**Komut:**
```powershell
dotnet ef database update --project RandevuWeb.csproj
dotnet run
```

### ✅ **2. Seçenek (Otomatik)** - EN KOLAY
**Ne zaman kullanılır:**
- Hızlı test için
- Development ortamında
- Tek seferlik kurulum için

**Komut:**
```powershell
dotnet run
```

### ⚠️ **3. Seçenek (Manuel SQL)** - ÖZEL DURUMLAR
**Ne zaman kullanılır:**
- Migration çalışmıyorsa
- Özel SQL script'i gerekiyorsa
- Veritabanı yöneticisi tarafından yapılacaksa

---

## 🚀 Hızlı Başlangıç (Önerilen)

```powershell
# 1. Migration'ları uygula
dotnet ef database update --project RandevuWeb.csproj

# 2. Uygulamayı çalıştır (JSON verileri otomatik aktarılır)
dotnet run
```

Bu kadar! Veritabanı hazır ve veriler aktarılmış olacak.

---

## 🔍 Kontrol Etmek İçin

**SQL Server Management Studio'da:**
```sql
-- Veritabanını kontrol et
USE randevu_sistemi;
GO

-- Tabloları listele
SELECT * FROM INFORMATION_SCHEMA.TABLES;
GO

-- Foreign key'leri kontrol et
SELECT 
    fk.name AS ForeignKey,
    tp.name AS ParentTable,
    cp.name AS ParentColumn,
    tr.name AS ReferencedTable,
    cr.name AS ReferencedColumn
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.tables AS tp ON fkc.parent_object_id = tp.object_id
INNER JOIN sys.columns AS cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
INNER JOIN sys.tables AS tr ON fkc.referenced_object_id = tr.object_id
INNER JOIN sys.columns AS cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id;
GO

-- Veri sayılarını kontrol et
SELECT 'Doctors' AS TableName, COUNT(*) AS RowCount FROM Doctors
UNION ALL
SELECT 'Patients', COUNT(*) FROM Patients
UNION ALL
SELECT 'Appointments', COUNT(*) FROM Appointments;
GO
```

