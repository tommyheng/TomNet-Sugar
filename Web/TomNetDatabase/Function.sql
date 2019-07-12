CREATE TABLE [dbo].[Function]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[Source] NVARCHAR(50) NOT NULL , 
	[IsWebApi] BIT NOT NULL,
	[IsController] BIT NOT NULL, 
    [Name] NVARCHAR(200) NULL, 
    [Area] NVARCHAR(50) NULL, 
    [Controller] NVARCHAR(50) NULL, 
    [Action] NVARCHAR(50) NULL, 
    [AccessType] INT NOT NULL, 
    [IsLocked] BIT NOT NULL
)
