using BookRadarLibrary.DataAccess;
using BookRadarLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BookRadarApi.Controlers;

[Route("api/[controller]")]
[ApiController]
public class WatchListController : ControllerBase
{
    private readonly IWatchListData _data;
    private readonly ILogger<WatchListController> _logger;
    private readonly IAccessLogData _accessData;

    public WatchListController(IWatchListData data, ILogger<WatchListController> logger, IAccessLogData accessData)
    {
        _data = data;
        _logger = logger;
        _accessData = accessData;
    }

    

    [HttpPost("watch")]
    public async Task<IActionResult> AddToWatchList([FromBody] BookModel book)
    {
        try
        {
            string ? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _logger.LogError("User Id could not be matched. Watchlist att to list");
                return StatusCode(500, "An error occurred while processing your request.");

            }
            await _data.CreateWatchList(userId, book.Id);

            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!userRoles.Contains("Demigod"))
            {
                if (userId is not null)
                {
                    var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    // Log login - for reporting :)
                    AccessLogModel accessLog = new AccessLogModel { UserId = userId, UserName = username!, EventAction = "Watch Book", IdType = "Book", TypeId = book.Id };
                    _accessData.WriteAccessLogEntry(accessLog);
                }
            }

            return Ok("Added to watch list");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding to watch list");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }


    [HttpPut("archive")]
    public async Task<IActionResult> ArchiveWatchList([FromBody] BookModel book)
    {
        try
        {
            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                _logger.LogError("User Id could not be matched. Watchlist add to list");
                return StatusCode(500, "An error occurred while processing your request.");

            }
            await _data.ArchiveWatchList(userId, book.Id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while archiving in watch list");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
