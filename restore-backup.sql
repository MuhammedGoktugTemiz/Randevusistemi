-- =============================================
-- Veritabani Restore Script'i
-- Kullanim: SQL Server Management Studio'da calistirin
-- =============================================

USE master;
GO

-- Backup dosya yolu (production sunucuda bu yolu guncelleyin)
DECLARE @BackupPath NVARCHAR(500) = 'C:\Users\Muhammed Göktuğ\Desktop\Yeni klasör (3)\Yeni klasör (2)\MSSQL16.SQLEXPRESS04\MSSQL\Backup\randevu_sistemi.bak';
DECLARE @DatabaseName NVARCHAR(100) = 'randevu_sistemi';
DECLARE @DataPath NVARCHAR(500) = 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\';
DECLARE @LogPath NVARCHAR(500) = 'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\';

-- Mevcut baglantilari kes
IF EXISTS (SELECT * FROM sys.databases WHERE name = @DatabaseName)
BEGIN
    ALTER DATABASE [randevu_sistemi]
    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
END
GO

-- Veritabanini restore et
RESTORE DATABASE [randevu_sistemi]
FROM DISK = @BackupPath
WITH REPLACE,
     MOVE 'randevu_sistemi' TO @DataPath + 'randevu_sistemi.mdf',
     MOVE 'randevu_sistemi_log' TO @LogPath + 'randevu_sistemi_log.ldf',
     STATS = 10;
GO

-- Coklu kullanici moduna geri don
ALTER DATABASE [randevu_sistemi]
SET MULTI_USER;
GO

PRINT 'Veritabani basariyla restore edildi!';
GO

-- Kontrol sorgusu
USE [randevu_sistemi];
GO

SELECT 
    'Doctors' AS TableName, 
    COUNT(*) AS RowCount 
FROM [Doctors]
UNION ALL
SELECT 'Patients', COUNT(*) FROM [Patients]
UNION ALL
SELECT 'Appointments', COUNT(*) FROM [Appointments];
GO

