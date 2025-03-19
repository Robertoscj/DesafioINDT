IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TravelRoutes')
BEGIN
    CREATE DATABASE TravelRoutes;
END
GO

USE TravelRoutes;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Routes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Routes](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Origin] [nvarchar](50) NOT NULL,
        [Destination] [nvarchar](50) NOT NULL,
        [Cost] [decimal](18, 2) NOT NULL,
        CONSTRAINT [PK_Routes] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Insert sample data
IF NOT EXISTS (SELECT * FROM [dbo].[Routes])
BEGIN
    INSERT INTO [dbo].[Routes] ([Origin], [Destination], [Cost]) VALUES
    ('GRU', 'BRC', 10),
    ('BRC', 'SCL', 5),
    ('GRU', 'CDG', 75),
    ('GRU', 'SCL', 20),
    ('GRU', 'ORL', 56),
    ('ORL', 'CDG', 5),
    ('SCL', 'ORL', 20);
END
GO 