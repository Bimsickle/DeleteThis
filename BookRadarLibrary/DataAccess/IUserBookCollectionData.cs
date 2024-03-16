using BookRadarLibrary.Models;

namespace BookRadarLibrary.DataAccess
{
    public interface IUserBookCollectionData
    {
        Task AddBookToCollection(string userAccountId, Guid bookId, string edition);
        Task DeleteBookCollection(Guid id);
        Task EditBookCollection(Guid id, string edition);
        Task<IEnumerable<UserBookCollectionModel>> GetCollectionByUser(string userAccountId);
        Task<IEnumerable<BookCollectorsModel>> GetCollectionsByBook(Guid bookId);
    }
}