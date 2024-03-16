CREATE TABLE [dbo].[AccountValidation]
(
	[CreatedDate] datetime2(7) NOT NULL DEFAULT GETUTCDATE(),
	[UserId] NVARCHAR(450) NOT NULL,
	[UserEmail] NVARCHAR(450) NOT NULL
)
