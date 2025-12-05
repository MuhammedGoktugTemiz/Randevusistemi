-- =============================================
-- Veritabani Backup Olusturma Script'i
-- Kullanim: SQL Server Management Studio'da calistirin
-- =============================================

USE master;
GO

-- Backup klasorunu belirleyin
DECLARE @BackupPath NVARCHAR(500) = 'C:\Users\Muhammed Göktuğ\Desktop\Yeni klasör (3)\Yeni klasör (2)\MSSQL16.SQLEXPRESS04\MSSQL\Backup\randevu_sistemi_' + CONVERT(VARCHAR(20), GETDATE(), 112) + '_' + REPLACE(CONVERT(VARCHAR(20), GETDATE(), 108), ':', '') + '.bak';
DECLARE @DatabaseName NVARCHAR(100) = 'randevu_sistemi';

-- Veritabani var mi kontrol et
IF EXISTS (SELECT * FROM sys.databases WHERE name = @DatabaseName)
BEGIN
    -- Backup olustur
    BACKUP DATABASE [randevu_sistemi]
    TO DISK = @BackupPath
    WITH FORMAT,
         NAME = 'Randevu Sistemi Full Backup',
         DESCRIPTION = 'Randevu Sistemi Veritabani Backup',
         COMPRESSION,
         STATS = 10;
    
    PRINT 'Backup basariyla olusturuldu: ' + @BackupPath;
END
ELSE
BEGIN
    PRINT 'HATA: Veritabani bulunamadi: ' + @DatabaseName;
    PRINT 'Lutfen once database-script.sql dosyasini calistirarak veritabanini olusturun.';
END
GO

-- Alternatif: Manuel backup klasorunu belirtmek icin
-- BACKUP DATABASE [randevu_sistemi]
-- TO DISK = 'C:\Backup\randevu_sistemi.bak'
-- WITH FORMAT, COMPRESSION;

