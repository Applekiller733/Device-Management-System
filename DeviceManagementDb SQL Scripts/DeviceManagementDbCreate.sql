IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'DeviceManagementDb')
BEGIN
    CREATE DATABASE DeviceManagementDb;
END
GO

USE DeviceManagementDb;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
        [Name] NVARCHAR(100) NOT NULL,
        [Role] NVARCHAR(50) NOT NULL,
        [Location] NVARCHAR(100) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Devices]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Devices] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
        [Name] NVARCHAR(100) NOT NULL,
        [Manufacturer] NVARCHAR(100) NOT NULL,
        [Type] INT NOT NULL, 
        [OS] INT NOT NULL,   
        [OSVersion] NVARCHAR(50) NOT NULL,
        [Processor] NVARCHAR(100) NOT NULL,
        [RamAmount] NVARCHAR(50) NOT NULL,
        [Description] NVARCHAR(1000) NULL,
        
        [AssignedUserId] UNIQUEIDENTIFIER NULL,
        CONSTRAINT [FK_Devices_Users] FOREIGN KEY ([AssignedUserId]) 
        REFERENCES [dbo].[Users]([Id]) ON DELETE SET NULL 
    );
END
GO