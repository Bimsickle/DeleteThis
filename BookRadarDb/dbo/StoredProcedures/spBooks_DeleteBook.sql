create procedure [dbo].[spBooks_DeleteBook]
	@BookId UNIQUEIDENTIFIER
as
begin
	UPDATE [dbo].[Book] 
	SET IsDeleted = 1
	WHERE
     Id = @BookId;

     UPDATE [dbo].[WatchList]
     SET IsDeleted = 1
	    WHERE
         BookId = @BookId;


    WITH OrphanCounts AS (
        SELECT
            COUNT(wl.Id) + COUNT(g.Id)  AS orphans
        FROM
            [dbo].[Book] b
            LEFT JOIN [dbo].[WatchList] wl ON wl.BookId = b.Id
            LEFT JOIN BookGenre g ON g.BookId = b.Id
        WHERE
            b.Id = @BookId
	
    )

    DELETE FROM [dbo].[Book]
    WHERE
        Id = @BookId
        AND (
            SELECT orphans FROM OrphanCounts
        ) = 0;
end