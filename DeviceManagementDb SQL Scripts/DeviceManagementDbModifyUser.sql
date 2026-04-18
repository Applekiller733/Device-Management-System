
ALTER TABLE Users
ADD Email NVARCHAR(255) NULL,
    PasswordHash NVARCHAR(255) NULL;
GO

UPDATE Users
SET Email = LOWER(REPLACE(Name, ' ', '.')) + '@company.com',
    PasswordHash = 'needs_password_reset'
WHERE Email IS NULL;
GO

ALTER TABLE Users
ALTER COLUMN Email NVARCHAR(255) NOT NULL;
GO

ALTER TABLE Users
ALTER COLUMN PasswordHash NVARCHAR(255) NOT NULL;
GO

ALTER TABLE Users
ADD CONSTRAINT UQ_Users_Email UNIQUE (Email);
GO
