CREATE PROCEDURE [dbo].[spPublisher_CreateWithId]
	@Id UNIQUEIDENTIFIER,
	@PublishingHouse NVARCHAR(255)
AS

BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Publisher] ([Id],[PublishingHouse])
	VALUES (@Id, @PublishingHouse);
END