CREATE PROCEDURE [dbo].[spUserAccount_Verify]
	@UserId NVARCHAR(450) ,
	@UserEmail  NVARCHAR(450)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT UserId Id 
	FROM
	[dbo].[AccountValidation] 
	WHERE  UserId = @UserId AND UserEmail = @UserEmail;
END
