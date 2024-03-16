using BookRadarLibrary.DataAccess;
using BookRadarLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookRadarApi.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleData _data;
        private readonly ILogger<NewsArticleController> _logger;
        private readonly IAccessLogData _accessData;

        public NewsArticleController(INewsArticleData data, ILogger<NewsArticleController> logger, IAccessLogData accessData)
        {
            _data = data;
            _logger = logger;
            _accessData = accessData;
        }
        // GET: api/<NewsArticleController>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<NewsArticleModel>>> GetAllNews()
        {
            var news =  await _data.GetNewsArticles();
            return Ok(news);
        }
        [HttpGet("Admin")]
        [Authorize(Roles = "Demigod")]
        public async Task<ActionResult<List<NewsArticleModel>>> GetAllNewsAdmin()
        {
            //return Extra paramerters
            var news = await _data.GetNewsArticlesAdmin();
            return Ok(news);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<NewsArticleModel>> GetNewsArticle(Guid id)
        {
            var news = await _data.GetOneNewsArticle(id);

            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!userRoles.Contains("Demigod"))
            {
                string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string username;
            if (userId is not null)
            {
                username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            }
            else
            {
                userId = "Anonymous";
                username = "Anonymous";
            }
                // Log login - for reporting :)
                AccessLogModel accessLog = new AccessLogModel { UserId = userId, UserName = username!, EventAction = "View News", IdType = "News", TypeId = id };
                _accessData.WriteAccessLogEntry(accessLog);
            }
            return Ok(news);
        }

        // POST api/<NewsArticleController>
        [HttpPost]
        [Authorize(Roles = "Demigod")]
        public async Task<ActionResult> CreateNewsArticle([FromBody] NewsArticleModel news)
        {            
            if (news == null)
            {
                return BadRequest("Article was empty");
            }
            else
            {
                var userIdText = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdText))
                {
                    return BadRequest();
                }
                news.AuthorId = userIdText;
                await _data.CreateNewsArticle(news);
                return Ok();
            }
        }

        // PUT api/<NewsArticleController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Demigod")]
        public async Task<IActionResult> UpdateNewsArticle(Guid id, [FromBody] NewsArticleModel news)
        {
            await _data.UpdateNewsArticle(news);
            return Ok();
        }

        //// DELETE api/<NewsArticleController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
