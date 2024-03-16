CREATE PROCEDURE [dbo].[spUserBookCollection_Delete]
	@Id UNIQUEIDENTIFIER

AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM  [dbo].[UserBookCollection]
	
	WHERE Id = @Id;
END