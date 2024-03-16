CREATE PROCEDURE [dbo].[spUserAccount_CreateRefreshToken]
	@UserId NVARCHAR(450),
	@Token NVARCHAR(450),
	@ExpDate DATETIME,
	@Device NVARCHAR(255) = 'Primary'

AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM [dbo].[UserSession] WHERE UserId = @UserId AND Device = @Device)
    BEGIN
        -- Update the existing record with the new token and expiration date
        UPDATE [dbo].[UserSession]
        SET RefreshToken = @Token,
            ExprationDateUtc = @ExpDate
        WHERE UserId = @UserId AND Device = @Device;
    END
    ELSE
    BEGIN
        -- Insert a new record if no matching record exists
        INSERT INTO [dbo].[UserSession] (UserId, RefreshToken, ExprationDateUtc, Device)
        VALUES (@UserId, @Token, @ExpDate, @Device);
    END

END
