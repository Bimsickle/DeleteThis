using BookRadarLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRadarLibrary.DataAccess;

public class AccessLogData : IAccessLogData
{
    private readonly ISqlDataAccess _sql;

    public AccessLogData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public Task<IEnumerable<AccessLogModel>> GetAccessLogByDateRange(DateTime startDate, DateTime endDate)
    {
        // return list of user account Id's -- get details on front end with identity
        string startString = startDate.ToString("yyyyMMdd HH:mm:ss");
        string endString = endDate.ToString("yyyyMMdd HH:mm:ss");
        return _sql.LoadData<AccessLogModel, dynamic>(
            "dbo.spAccessLog_GetLogsByDate",
            new { StartDate = startString, EndDate = endString }, "Default");
    }
    public Task<IEnumerable<AccessLogModel>> GetAccessLogById(DateTime startDate, DateTime endDate, Guid id)
    {
        // return list of user account Id's -- get details on front end with identity
        string startString = startDate.ToString("yyyyMMdd HH:mm:ss");
        string endString = endDate.ToString("yyyyMMdd HH:mm:ss");
        return _sql.LoadData<AccessLogModel, dynamic>(
            "dbo.spAccessLog_GetById",
            new { StartDate = startString, EndDate = endString, Id = id }, "Default");
    }
    public Task<IEnumerable<AccessLogModel>> GetAccessLogByAction(DateTime startDate, DateTime endDate, string action)
    {
        // return list of user account Id's -- get details on front end with identity
        string startString = startDate.ToString("yyyyMMdd HH:mm:ss");
        string endString = endDate.ToString("yyyyMMdd HH:mm:ss");
        return _sql.LoadData<AccessLogModel, dynamic>(
            "dbo.spAccessLog_GetByAction",
            new { StartDate = startString, EndDate = endString, Action = action }, "Default");
    }
    public Task<IEnumerable<AccessLogModel>> GetAccessLogByUserId(DateTime startDate, DateTime endDate, string id)
    {
        // return list of user account Id's -- get details on front end with identity
        string startString = startDate.ToString("yyyyMMdd HH:mm:ss");
        string endString = endDate.ToString("yyyyMMdd HH:mm:ss");
        return _sql.LoadData<AccessLogModel, dynamic>(
            "dbo.spAccessLog_GetByUserId",
            new { StartDate = startString, EndDate = endString, UserId = id }, "Default");
    }
    public Task WriteAccessLogEntry(AccessLogModel accessLog)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spAccessLog_CreateLog",
            new
            {
                accessLog.UserId,
                accessLog.UserName,
                accessLog.EventAction,
                accessLog.IdType,
                accessLog.TypeId
            },
            "Default");
    }

}
