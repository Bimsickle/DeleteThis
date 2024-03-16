using BookRadarLibrary.Models;
using System.Net;

namespace BookRadarLibrary.DataAccess;

public class WatchListData : IWatchListData
{
    private readonly ISqlDataAccess _sql;
    public WatchListData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public Task<IEnumerable<StringIdModel>> GetWatchersByBook(Guid bookId)
    {
        // return list of user account Id's -- get details on front end with identity
        return _sql.LoadData<StringIdModel, dynamic>(
            "dbo.spWatchList_GetAccountIdByBook",
            new { BookId = bookId}, "Default");
    }

    public Task ArchiveWatchList(string accountId, Guid bookId)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spWatchList_Archive",
            new { UserAccountId = accountId, BookId = bookId },
            "Default");
    }

    public Task CreateWatchList(string accountId, Guid bookId)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spWatchList_Create",
            new { UserAccount = accountId, BookId = bookId },
            "Default");
    }
}
