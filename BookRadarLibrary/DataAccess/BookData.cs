using BookRadarLibrary.Models;
using Microsoft.Identity.Client;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookRadarLibrary.DataAccess;

public class BookData : IBookData

{
    private readonly ISqlDataAccess _sql;
    public BookData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public async Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize, int days, string? search = null, string? accountId = null, string? roles = null)
    {
        var books = await _sql.LoadData<BookModel, dynamic>(
        "dbo.spBooks_GetAllBooks",
        new { AccountId = accountId, PageNumber = pageNumber, PageSize = pageSize ,Search = search, Days = days, Roles = roles}, "Default");

        // Get Genres for all books
        List<Guid> bookIds = books.Select(book => book.Id).ToList();         
        var genres = await _sql.LoadData<GenreModel, dynamic>(
            "dbo.spBook_GenreGetAll",
            new { BookIds = string.Join(",", bookIds) }, "Default");

        // Add Genre list to Book List
        foreach (var book in books)
        {
            book.GenreList = genres.Where(g => g.BookId == book.Id).ToList();
        }

        return books;

    }

    public async Task<BookModel?> GetOneBook(Guid bookId, string? accountId = null, string? roles = null)
    {
        var results = await _sql.LoadData<BookModel, dynamic>(
            "dbo.spBooks_GetOneBook",
            new { BookId = bookId , AccountId = accountId, Roles = roles },
            "Default");

        // Get Genres for all books
        List<Guid> bookIds = results.Select(book => book.Id).ToList();
        var genres = await _sql.LoadData<GenreModel, dynamic>(
            "dbo.spBook_GenreGetAll",
            new { BookIds = string.Join(",", bookIds) }, "Default");

        // Add Genre list to Book List
        foreach (var book in results)
        {
            book.GenreList = genres.Where(g => g.BookId == book.Id).ToList();
        }

        return results.FirstOrDefault();
    }

    public Task UpdateBook(BookModel book)
    {
        if (book.GenreList != null)
        {
            foreach (var genre in book.GenreList)
            {
                var newGenreResult = GetCreateGenre(genre.Genre!);
                GenreModel newGenre = newGenreResult.Result;

                if (genre.IsDeleted == false)
                {
                    // create
                    _sql.SaveData<dynamic>("dbo.spGenre_CreateBookGenre",
                                            new { BookId = book.Id, GenreId = newGenre.Id }, "Default");
                }
                else
                {
                    // Delete
                    _sql.SaveData<dynamic>("dbo.spGenre_DeleteBookGenre",
                                            new { BookId = book.Id, GenreId = newGenre.Id }, "Default");
                }
            }
        }

        return _sql.SaveData<dynamic>(
            "dbo.spBooks_UpdateBook",
            new
            {
            BookTitle = book.BookTitle,
            Author = book.Author,
            PublishingHouseId = book.PublishingHouseId,
            Url = book.Url,
            ReleaseDate = book.ReleaseDate,
            IsReleased = book.IsReleased,
            IsFeatured = book.IsFeatured,
            TotalRunSize = book.TotalRunSize,
            EditionStandard = book.EditionStandard,
            EditionNumbered = book.EditionNumbered,
            EditionLettered = book.EditionLettered,
            EditionDelux = book.EditionDelux,
            ImageCover = book.ImageCover,
            Imagefeature = book.ImageFeature,
            Description = book.Description,
            Id = book.Id // Only used to identify the record to update, not to update the field itself.
            },
            "Default");
    }

    public Task DeleteBook(Guid bookId)
    {
        return _sql.SaveData<dynamic>(
               "dbo.spBooks_DeleteBook",
               new { BookId = bookId },
               "Default");        
    }

    public  Task CreateBook(BookModel book)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spBooks_CreateBook",
            new {
                book.BookTitle,
                book.Author,
                book.PublishingHouseId,
                book.Url,
                book.ReleaseDate,
                book.IsReleased,
                book.IsFeatured,
                book.TotalRunSize,
                book.EditionStandard,
                book.EditionNumbered,
                book.EditionLettered,
                book.EditionDelux,
                book.ImageCover,
                book.ImageFeature,
                book.Description
            },
                "Default");
    }

    public async Task<IEnumerable<BookModel>> GetFeaturedBooks()
    {
        var books = await _sql.LoadData<BookModel, dynamic>(
        "dbo.spBooks_GetFeatured",
        new {  }, "Default");

        // Get Genres for all books
        List<Guid> bookIds = books.Select(book => book.Id).ToList();
        var genres = await _sql.LoadData<GenreModel, dynamic>(
            "dbo.spBook_GenreGetAll",
            new { BookIds = string.Join(",", bookIds) }, "Default");

        // Add Genre list to Book List
        foreach (var book in books)
        {
            book.GenreList = genres.Where(g => g.BookId == book.Id).ToList();
        }

        return books;
    }

    /**** GENRE ****/
    public async Task<IEnumerable<GenreModel>> GetAllGenres()
    {
        var genres = await _sql.LoadData<GenreModel, dynamic>(
        "dbo.spGenre_GetAllGenres",
        new {  }, "Default");
        return genres;
    }
    public async Task <GenreModel> GetCreateGenre(string genre)
    {
        var result = await _sql.LoadData<GenreModel, dynamic>(
        "dbo.spGenre_GetOrCreateGenreByGenre",
        new { Genre = genre }, "Default");
        
        return result.FirstOrDefault()!;        
    }
    public async Task<IEnumerable<GenreModel>> GetBookGenres(Guid idg)
    {
        string id = idg.ToString();
        var result = await _sql.LoadData<GenreModel, dynamic>(
            "dbo.spBook_GenreGetAll",
            new { BookIds = id }, "Default");

        return result;
    }
    public Task AddGenreToBook (Guid bookId, Guid genreId)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spGenre_CreateBookGenre",
            new { BookId = bookId, GenreId = genreId }, "Default");
    }

    /***** PUBLISHERS *****/
    public async Task<IEnumerable<PublisherModel>> GetAllPublishers()
    {
        var publishers = await _sql.LoadData<PublisherModel, dynamic>(
        "dbo.spPublisher_SelectAll",
        new { }, "Default");
        return publishers;
    }
    public Task CreatePublisher(PublisherModel publisher)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spPublisher_CreateWithId",
            new
            {
                publisher.Id,
                publisher.PublishingHouse
            },
                "Default");
    }
}
