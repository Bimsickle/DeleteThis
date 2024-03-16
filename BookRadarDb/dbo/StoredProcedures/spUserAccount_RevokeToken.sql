CREATE PROCEDURE [dbo].[spUserAccount_RevokeToken]
	@UserId NVARCHAR(450),
	@Device NVARCHAR(255) 

AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[UserSession]
	WHERE 
	UserId =  @UserId AND Device =  @Device;

END
