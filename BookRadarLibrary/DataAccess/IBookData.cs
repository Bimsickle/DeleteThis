using BookRadarLibrary.Models;

namespace BookRadarLibrary.DataAccess;

public interface IBookData
{
    Task CreateBook(BookModel book);
    Task DeleteBook(Guid bookId);
    Task<IEnumerable<BookModel>> GetAllBooks(int pageNumber, int pageSize, int days, string? search = null, string? accountId = null, string? roles = null);
    Task<BookModel?> GetOneBook(Guid bookId, string? accountId = null, string? roles = null);
    Task UpdateBook(BookModel book);

    /** GENRES **/
    Task<IEnumerable<GenreModel>> GetAllGenres();
    Task AddGenreToBook(Guid bookId, Guid genreId);
    Task<GenreModel> GetCreateGenre(string genre);
    Task<IEnumerable<BookModel>> GetFeaturedBooks();
    Task<IEnumerable<PublisherModel>> GetAllPublishers();
    Task CreatePublisher(PublisherModel publisher);
}