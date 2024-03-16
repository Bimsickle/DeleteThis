using Microsoft.AspNetCore.Mvc;
using BookRadarLibrary.Models;
using BookRadarLibrary.DataAccess;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BookRadarApi.Controlers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookData _data;
    private readonly ILogger<BooksController> _logger;
    private readonly IAccessLogData _accessData;

    public BooksController(IBookData data, ILogger<BooksController> logger, IAccessLogData accessData)
    {
        _data = data;
        _logger = logger;
        _accessData = accessData;
    }       

    // GET: api/Books
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<BookModel>>> GetAllBooks(int pageNumber = 1, int pageSize = 200, int days = 21, string? search = null)
    {
        try
        {
            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            string roles = string.Join(",", userRoles);
            if (string.IsNullOrEmpty(roles))
            {
                roles = null!;
            }

            var books = await _data.GetAllBooks(pageNumber: pageNumber, pageSize: pageSize, days: days, search: search, accountId: userId, roles:roles);

            return Ok(books.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching books");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // GET api/Books/5
    [HttpGet("{bookId}")]
    [AllowAnonymous]
    public async Task<ActionResult<BookModel>> GetOneBook(Guid bookId)
    {
        //var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        try
        {
            string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            string roles = string.Join(",", userRoles);
            if (string.IsNullOrEmpty(roles))
            {
                roles = null!;
            }

            var output = await _data.GetOneBook(bookId, accountId: userId, roles: roles);
            if (!userRoles.Contains("Demigod"))
            {
                string username;
                if(userId is not null)
                {
                    username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                }
                else
                {
                    userId = "Anonymous";
                    username = "Anonymous";
                }
                // Log login - for reporting :)
                AccessLogModel accessLog = new AccessLogModel { UserId = userId, UserName = username!, EventAction = "View Book", IdType = "Book", TypeId = bookId  };
                _accessData.WriteAccessLogEntry(accessLog);
            }

            return Ok(output);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching books");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /**** GENRE ****/
    [HttpGet("Genre")]
    [Authorize(Roles = "Demigod,Publisher")]
    public async Task<ActionResult<GenreModel>> GetAllGenres()
    {
        var result = await _data.GetAllGenres();

        return Ok(result);
    }
}
