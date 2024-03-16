CREATE PROCEDURE [dbo].[spUserAccount_Register]
	@UserId NVARCHAR(450) ,
	@UserEmail  NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[AccountValidation] (UserId, UserEmail)
	VALUES (@UserId, @UserEmail)
END
