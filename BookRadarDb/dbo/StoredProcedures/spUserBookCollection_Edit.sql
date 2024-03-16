CREATE PROCEDURE [dbo].[spUserBookCollection_Edit]
	@Id UNIQUEIDENTIFIER,
	@Edition NVARCHAR(64)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[UserBookCollection]
	SET BookEdition = @Edition

	WHERE Id = @Id;
END
