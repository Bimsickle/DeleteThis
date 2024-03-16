using BookRadarLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRadarLibrary.DataAccess;

public class UserBookCollectionData : IUserBookCollectionData
{
    private readonly ISqlDataAccess _sql;
    public UserBookCollectionData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public async Task<IEnumerable<UserBookCollectionModel>> GetCollectionByUser(string userAccountId)
    {
        var books = await _sql.LoadData<UserBookCollectionModel, dynamic>(
        "dbo.spUserBookCollection_GetAllByUserId",
        new { UserAccountId = userAccountId }, "Default");

        // Get Genres for all books
        if (books.Count() >0)
        {
            List<Guid> bookIds = books.Select(book => book.BookId).ToList();
            var genres = await _sql.LoadData<GenreModel, dynamic>(
                "dbo.spBook_GenreGetAll",
                new { BookIds = string.Join(",", bookIds) }, "Default");

            // Add Genre list to Book List
            foreach (var book in books)
            {
                book.GenreList = genres.Where(g => g.BookId == book.BookId).ToList();
            }
        }

        return books;
    }
    public async Task<IEnumerable<BookCollectorsModel>> GetCollectionsByBook(Guid bookId)
    {
        var collectors = await _sql.LoadData<BookCollectorsModel, dynamic>(
        "dbo.spUserBookCollection_GetAllByBookId",
        new { BookId = bookId }, "Default");

        return collectors;
    }

    public async Task AddBookToCollection(string userAccountId, Guid bookId, string edition)
    {
        await _sql.SaveData<dynamic>("dbo.spUserBookCollection_Create",
                new { BookId = bookId, UserAccountId = userAccountId, Edition = edition }, "Default");
    }
    public async Task EditBookCollection(Guid id, string edition)
    {
        await _sql.SaveData<dynamic>("dbo.spUserBookCollection_Edit",
                new { Id = id, Edition = edition }, "Default");
    }
    public async Task DeleteBookCollection(Guid id)
    {
        await _sql.SaveData<dynamic>("dbo.spUserBookCollection_Delete",
                new { Id = id }, "Default");
    }
}
