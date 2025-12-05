-- =============================================
-- Randevu Sistemi Veritabani Olusturma Script'i
-- Migration: InitialCreate
-- Tarih: 2024-12-04
-- =============================================

-- Veritabani olustur (eğer yoksa)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'randevu_sistemi')
BEGIN
    CREATE DATABASE [randevu_sistemi];
END
GO

USE [randevu_sistemi];
GO

-- Migration History Tablosu (EF Core için)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[__EFMigrationsHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END
GO

-- =============================================
-- TABLOLAR
-- =============================================

-- Doctors Tablosu
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Doctors]') AND type in (N'U'))
BEGIN
    CREATE TABLE [Doctors] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Specialty] nvarchar(200) NOT NULL,
        [Username] nvarchar(100) NOT NULL,
        [Password] nvarchar(500) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_Doctors] PRIMARY KEY ([Id])
    );
    
    -- Username Unique Index
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Doctors_Username] ON [Doctors] ([Username]);
END
GO

-- Patients Tablosu
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Patients]') AND type in (N'U'))
BEGIN
    CREATE TABLE [Patients] (
        [Id] int NOT NULL IDENTITY,
        [FullName] nvarchar(200) NOT NULL,
        [TcKimlik] nvarchar(11) NOT NULL,
        [BirthDate] datetime2 NULL,
        [Gender] nvarchar(max) NOT NULL,
        [Phone] nvarchar(20) NOT NULL,
        [Email] nvarchar(100) NOT NULL,
        [Address] nvarchar(500) NOT NULL,
        [BloodType] nvarchar(10) NOT NULL,
        [Allergies] nvarchar(max) NOT NULL,
        [EmergencyContact] nvarchar(max) NOT NULL,
        [PhotoUrl] nvarchar(max) NOT NULL,
        [Status] nvarchar(50) NOT NULL,
        [StatusColor] nvarchar(20) NOT NULL,
        [LastVisit] nvarchar(max) NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        [ToothNumbers] nvarchar(max) NOT NULL,
        [ProcedureDetails] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Patients] PRIMARY KEY ([Id])
    );
    
    -- TcKimlik Index
    CREATE NONCLUSTERED INDEX [IX_Patients_TcKimlik] ON [Patients] ([TcKimlik]);
END
GO

-- Appointments Tablosu
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Appointments]') AND type in (N'U'))
BEGIN
    CREATE TABLE [Appointments] (
        [Id] int NOT NULL IDENTITY,
        [PatientId] int NOT NULL,
        [PatientName] nvarchar(200) NOT NULL,
        [DoctorId] int NOT NULL,
        [DoctorName] nvarchar(200) NOT NULL,
        [DoctorSpecialty] nvarchar(200) NOT NULL,
        [Start] datetime2 NOT NULL,
        [DurationMinutes] int NOT NULL,
        [Procedure] nvarchar(200) NOT NULL,
        [ToothNumbers] nvarchar(100) NOT NULL,
        [ProcedureDetails] nvarchar(max) NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        [Status] nvarchar(50) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Appointments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Appointments_Doctors_DoctorId] FOREIGN KEY ([DoctorId]) REFERENCES [Doctors] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Appointments_Patients_PatientId] FOREIGN KEY ([PatientId]) REFERENCES [Patients] ([Id]) ON DELETE NO ACTION
    );
    
    -- Indexes
    CREATE NONCLUSTERED INDEX [IX_Appointments_DoctorId] ON [Appointments] ([DoctorId]);
    CREATE NONCLUSTERED INDEX [IX_Appointments_PatientId] ON [Appointments] ([PatientId]);
    CREATE NONCLUSTERED INDEX [IX_Appointments_Start] ON [Appointments] ([Start]);
END
GO

-- =============================================
-- MIGRATION HISTORY
-- =============================================

-- Migration kaydını ekle
IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204102053_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251204102053_InitialCreate', N'8.0.0');
END
GO

-- =============================================
-- KONTROL SORGULARI
-- =============================================

-- Tabloları kontrol et
SELECT 
    'Doctors' AS TableName, 
    COUNT(*) AS RowCount 
FROM [Doctors]
UNION ALL
SELECT 'Patients', COUNT(*) FROM [Patients]
UNION ALL
SELECT 'Appointments', COUNT(*) FROM [Appointments];
GO

-- Foreign Key'leri kontrol et
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
INNER JOIN sys.columns AS cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
WHERE tp.name IN ('Appointments', 'Doctors', 'Patients');
GO

PRINT 'Veritabani basariyla olusturuldu!';
GO

