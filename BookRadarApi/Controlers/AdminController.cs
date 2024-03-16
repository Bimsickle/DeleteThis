using BookRadarLibrary.DataAccess;
using BookRadarLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookRadarApi.Controlers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles="Demigod")]
public class AdminController : ControllerBase

{
    private readonly IWatchListData _data;
    private readonly IBookData _bookData;
    private readonly ILogger<AdminController> _logger;
    private readonly IAccessLogData _accessData;

    public AdminController(IWatchListData data, IBookData bookData,ILogger<AdminController> logger, IAccessLogData accessData)
    {
        _data = data;
        _logger = logger;
        _accessData = accessData;
        _bookData = bookData;
    }    

    /******* WATCH LIST ********/

    [HttpGet("WatchList/{bookId}")]
    public async Task<ActionResult<List<UserAccountModel>>> GetWatchersByBook(Guid bookId)
    {
        var output = await _data.GetWatchersByBook(bookId);
        return Ok(output);
    }

    /********* BOOK ********/

    // POST api/Books
    [HttpPost("Book")]
    public async Task<ActionResult> CreateBook([FromBody] BookModel book)
    {
        if (book == null)
        {
            return BadRequest("Book model was empty");
        }
        else
        {
            await _bookData.CreateBook(book);
            return Ok();
        }
    }

    // PUT api/Books/5
    [HttpPut("Book/{bookId}")]
    public async Task<IActionResult> UpdateBook([FromBody] BookModel book, Guid bookId)
    {
        if (book.Id == bookId)
        {
            await _bookData.UpdateBook(book);
            return Ok();
        }
        return BadRequest();
    }

    // DELETE api/Books/5
    [HttpDelete("Book/{bookId}")]
    public async Task<IActionResult> DeleteBook(Guid bookId)
    {
        await _bookData.DeleteBook(bookId);
        return Ok();
    }

    [HttpGet("Featured")]
    public async Task<ActionResult<List<UserAccountModel>>> GetFeaturedBooks()
    {
        var output = await _bookData.GetFeaturedBooks();
        return Ok(output);
    }

    /******* PUBLISHERS ***********/
    [HttpGet("Publishers")]
    public async Task<ActionResult<List<PublisherModel>>> GetAllPublishers()
    {
        var output = await _bookData.GetAllPublishers();
        return Ok(output);
    }

    [HttpPost("Publisher")]
    public async Task<ActionResult<List<PublisherModel>>> CreatePublisher([FromBody]PublisherModel publisher)
    {
        try
        {
            await _bookData.CreatePublisher(publisher);
            return Ok();
        }
        catch(Exception ex)
        {
            _logger.LogInformation($"Publisher could not be created. {ex}");
            return BadRequest();
        }
    }

    /******* ACCOUNT LOGGING ********/

    [HttpGet("AccessLog/DateSearch")]
    public async Task<ActionResult<List<AccessLogModel>>> GetAccessLogsByDate([FromQuery] DateSearchModel dates)
    {
        var output = await _accessData.GetAccessLogByDateRange(dates.StartDate, dates.EndDate);
        return Ok(output);
    }

    [HttpGet("AccessLog/{id}")]
    public async Task<ActionResult<List<AccessLogModel>>> GetAccessLogsById(Guid id, [FromQuery] DateSearchModel dates)
    {
        var output = await _accessData.GetAccessLogById(dates.StartDate, dates.EndDate, id);
        return Ok(output);
    }

    [HttpGet("AccessLog/{action}")]
    public async Task<ActionResult<List<AccessLogModel>>> GetAccessLogsByAction(string action, [FromQuery] DateSearchModel dates)
    {
        var output = await _accessData.GetAccessLogByDateRange(dates.StartDate, dates.EndDate);
        return Ok(output);
    }
    [HttpGet("AccessLog/User/{id}")]
    public async Task<ActionResult<List<AccessLogModel>>> GetAccessLogsByUserId(string id, [FromQuery] DateSearchModel dates)
    {
        var output = await _accessData.GetAccessLogByUserId(dates.StartDate, dates.EndDate, id);
        return Ok(output);
    }
}
