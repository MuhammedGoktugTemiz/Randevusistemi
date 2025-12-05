-- =============================================
-- Veritabani Backup Olusturma Script'i (Basit)
-- Backup dosyasi belirtilen klasore kaydedilecek
-- =============================================

USE master;
GO

-- Backup dosya yolu
DECLARE @BackupPath NVARCHAR(500) = 'C:\Users\Muhammed Göktuğ\Desktop\Yeni klasör (3)\Yeni klasör (2)\MSSQL16.SQLEXPRESS04\MSSQL\Backup\randevu_sistemi.bak';
DECLARE @DatabaseName NVARCHAR(100) = 'randevu_sistemi';

-- Veritabani var mi kontrol et
IF EXISTS (SELECT * FROM sys.databases WHERE name = @DatabaseName)
BEGIN
    PRINT 'Backup olusturuluyor...';
    PRINT 'Hedef: ' + @BackupPath;
    
    -- Backup olustur
    BACKUP DATABASE [randevu_sistemi]
    TO DISK = @BackupPath
    WITH FORMAT,
         NAME = 'Randevu Sistemi Full Backup',
         DESCRIPTION = 'Randevu Sistemi Veritabani Backup',
         COMPRESSION,
         STATS = 10;
    
    PRINT '';
    PRINT '========================================';
    PRINT 'Backup basariyla olusturuldu!';
    PRINT 'Dosya yolu: ' + @BackupPath;
    PRINT '========================================';
END
ELSE
BEGIN
    PRINT 'HATA: Veritabani bulunamadi: ' + @DatabaseName;
    PRINT 'Lutfen once database-script.sql dosyasini calistirarak veritabanini olusturun.';
END
GO

