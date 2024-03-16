CREATE TABLE [dbo].[UserSession]
(
	[CreatedDate] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
	[UserId] NVARCHAR(450) NOT NULL,
	[Device] NVARCHAR(255) NULL,
	[RefreshToken] NVARCHAR(450) NOT NULL,
	[ExprationDateUtc] datetime2(7) NOT NULL
)
