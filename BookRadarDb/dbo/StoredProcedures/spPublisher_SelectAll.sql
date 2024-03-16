CREATE PROCEDURE [dbo].[spPublisher_SelectAll]

AS

BEGIN
	SET NOCOUNT ON;
	SELECT 
        [Id]
      ,[CreatedDate]
      ,[PublishingHouse]
      ,[IsDeleted]
  FROM [dbo].[Publisher]
  WHERE [IsDeleted] <>1;

END
