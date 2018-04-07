CREATE TABLE [dbo].[Transaction]
(
	[IdTransaction] INT NOT NULL PRIMARY KEY identity,
	[Recesiver] NVARCHAR(MAX) NOT NULL,
	[Sender] NVARCHAR(MAX) NOT NULL,
	[Amount] DECIMAL NOT NULL,
	[Date] DATETIME NOT NULL,
	[Type] NVARCHAR(MAX) NOT NULL
)
