using BookRadarLibrary.Models;

namespace BookRadarLibrary.DataAccess;

public interface IWatchListData
{
    Task ArchiveWatchList(string accountId, Guid bookId);
    Task CreateWatchList(string accountId, Guid bookId);
    Task<IEnumerable<StringIdModel>> GetWatchersByBook(Guid bookId);
}