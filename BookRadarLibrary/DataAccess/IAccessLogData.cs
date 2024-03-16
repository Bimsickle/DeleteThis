using BookRadarLibrary.Models;

namespace BookRadarLibrary.DataAccess
{
    public interface IAccessLogData
    {
        Task<IEnumerable<AccessLogModel>> GetAccessLogByAction(DateTime startDate, DateTime endDate, string action);
        Task<IEnumerable<AccessLogModel>> GetAccessLogByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<AccessLogModel>> GetAccessLogById(DateTime startDate, DateTime endDate, Guid id);
        Task<IEnumerable<AccessLogModel>> GetAccessLogByUserId(DateTime startDate, DateTime endDate, string id);
        Task WriteAccessLogEntry(AccessLogModel accessLog);
    }
}