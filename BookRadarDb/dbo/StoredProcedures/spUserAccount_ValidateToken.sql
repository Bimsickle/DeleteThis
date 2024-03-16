CREATE PROCEDURE [dbo].[spUserAccount_ValidateToken]
	@UserId NVARCHAR(450),
	@Token NVARCHAR(450),
	@Device NVARCHAR(255) = 'Primary'

AS
BEGIN
	SET NOCOUNT ON;

    SELECT 1 [Validated]
    
    FROM [dbo].[UserSession] 
    
    WHERE 
	UserId = @UserId 
	AND Device = @Device
	AND RefreshToken = @Token 
	AND ExprationDateUtc > GETUTCDATE();    

END