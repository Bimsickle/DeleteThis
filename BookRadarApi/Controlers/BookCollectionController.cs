using BookRadarLibrary.DataAccess;
using BookRadarLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BookRadarApi.Controlers;

[Route("api/[controller]")]
[ApiController]
public class BookCollectionController : ControllerBase
{
    private readonly IUserBookCollectionData _data;
    private readonly ILogger<AdminController> _logger;

    public BookCollectionController(IUserBookCollectionData data, ILogger<AdminController> logger)
    {
        _data = data;
        _logger = logger;
    }
    // GET: api/BookCollection/User/Id
    [HttpGet("User")]
    public async Task<ActionResult<List<UserBookCollectionModel>>> GetCollectionByUser()
    {
        string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            _logger.LogError("User Id could not be matched. Watchlist att to list");
            return StatusCode(500, "An error occurred while processing your request.");

        }
        var output =  await _data.GetCollectionByUser(userId);
        return Ok(output);
    }
    // GET: api/BookCollection/Book/Id
    [HttpGet("Book/{bookId}")]
    public async Task<ActionResult<List<BookCollectorsModel>>> GetCollectorsByBook(Guid bookId)
    {
        var output = await _data.GetCollectionsByBook(bookId);
        return Ok(output);
    }


    // POST api/<BookCollection>
    [HttpPost]
    public async Task<ActionResult> CreateCollection(Guid bookId, string edition)
    {
        string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            _logger.LogError("User Id could not be matched. Watchlist att to list");
            return StatusCode(500, "An error occurred while processing your request.");

        }
        await _data.AddBookToCollection(userId, bookId, edition);
        return Ok();        
    }

    // PUT api/<BookCollectionController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> EditCollection(Guid id, string edition)
    {
        await _data.EditBookCollection(id, edition);
        return Ok();
    }

    // DELETE api/<BookCollectionController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult>DeleteCollection(Guid id)
    {
        await _data.DeleteBookCollection(id);
        return Ok();
    }
}
