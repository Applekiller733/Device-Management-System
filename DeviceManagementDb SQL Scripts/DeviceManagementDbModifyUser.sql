-- 1. Add the columns as NULLABLE first so the script doesn't crash on existing rows
ALTER TABLE Users
ADD Email NVARCHAR(255) NULL,
    PasswordHash NVARCHAR(255) NULL;
GO

-- 2. Backfill the existing seeded data
-- This generates a fake email based on their name (e.g., "John Doe" -> "john.doe@company.com")
-- and gives them a dummy password hash (they would need to be reset later in a real app)
UPDATE Users
SET Email = LOWER(REPLACE(Name, ' ', '.')) + '@company.com',
    PasswordHash = 'needs_password_reset'
WHERE Email IS NULL;
GO

-- 3. Now that no rows have NULLs, we can safely enforce the NOT NULL constraint
ALTER TABLE Users
ALTER COLUMN Email NVARCHAR(255) NOT NULL;
GO

ALTER TABLE Users
ALTER COLUMN PasswordHash NVARCHAR(255) NOT NULL;
GO

-- 4. Add a UNIQUE constraint to Email (Crucial for login systems!)
ALTER TABLE Users
ADD CONSTRAINT UQ_Users_Email UNIQUE (Email);
GO