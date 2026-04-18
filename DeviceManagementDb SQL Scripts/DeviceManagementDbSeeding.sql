USE DeviceManagementDb;
GO

DECLARE @FirstId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @SecondId UNIQUEIDENTIFIER   = '22222222-2222-2222-2222-222222222222';

IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @FirstId)
BEGIN
    INSERT INTO Users (Id, Name, Role, Location)
    VALUES (@FirstId, 'Popescu Ion', 'Software Engineer', 'Cluj Napoca Office');
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @SecondId)
BEGIN
    INSERT INTO Users (Id, Name, Role, Location)
    VALUES (@SecondId, 'Marian Cristian', 'QA Tester', 'Bucuresti Office');
END

IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'IonPhone')
BEGIN
    INSERT INTO [dbo].[Devices] ([Name], [Manufacturer], [Type], [OS], [OSVersion], [Processor], [RamAmount], [Description], [AssignedUserId])
    VALUES ('IonPhone', 'Apple', 1, 1, '17.4', 'A17 Pro', '8GB', 'Primary dev phone.', @FirstId);
END
GO