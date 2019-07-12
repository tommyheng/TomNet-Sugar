CREATE TABLE [dbo].[UserLogin]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL, 
    [IsLocked] BIT NOT NULL, 
    [CreateTime] DATETIME NOT NULL DEFAULT (getdate())
)
