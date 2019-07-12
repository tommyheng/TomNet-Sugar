CREATE TABLE [dbo].[UserInfo]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [NickName] NVARCHAR(50) NOT NULL, 
    [RealName] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [Telephone] NVARCHAR(50) NOT NULL, 
    [MobilePhone] NVARCHAR(50) NOT NULL, 
    [IdNumber] NVARCHAR(50) NOT NULL, 
    [Sex] BIT NOT NULL, 
    [Age] INT NOT NULL, 
    [Birthday] DATETIME NOT NULL
)
