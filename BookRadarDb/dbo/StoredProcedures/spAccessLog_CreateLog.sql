CREATE PROCEDURE [dbo].[spAccessLog_CreateLog]
	@UserId NVARCHAR(450) ,
	@UserName NVARCHAR(450),
	@EventAction NVARCHAR(255),
	@IdType NVARCHAR(100) = NULL,
	@TypeId UNIQUEIDENTIFIER = NULL
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[AccessLog]
	([UserId],[UserName],[EventAction],[IdType],[TypeId])
	VALUES 
	(@UserId,@UserName,@EventAction,@IdType,@TypeId)
END


