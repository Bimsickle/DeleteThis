using BookRadarLibrary.Models;

namespace BookRadarLibrary.DataAccess;

public class NewsArticleData : INewsArticleData
{
    private readonly ISqlDataAccess _sql;
    public NewsArticleData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public Task CreateNewsArticle(NewsArticleModel model)
    {
        return _sql.SaveData<dynamic>(
            "dbo.spNews_Create",
            new { model.Title, 
                model.Content, 
                model.AuthorId, 
                model.PublishDate, 
                model.Tags, 
                model.IsPublished,
                model.BannerImage, model.PreviewImage},
            "Default");
    }
    public async Task<IEnumerable<NewsArticleModel>> GetNewsArticles()
    {
        return await _sql.LoadData<NewsArticleModel, dynamic>(
            "dbo.spNews_GetAll",
            new { }, "Default");
    }
    public async Task<IEnumerable<NewsArticleModel>> GetNewsArticlesAdmin()
    {
        return await _sql.LoadData<NewsArticleModel, dynamic>(
            "dbo.spNews_GetAllAdmin",
            new { }, "Default");
    }
    public async Task<NewsArticleModel> GetOneNewsArticle(Guid id)
    {
        var news = await _sql.LoadData<NewsArticleModel, dynamic>(
            "dbo.spNews_GetById",
            new { Id = id }, "Default");
        return news.FirstOrDefault();
    }
    public Task UpdateNewsArticle(NewsArticleModel news)
    {
        return _sql.SaveData<dynamic>("dbo.spNewsArticle_EditPost",
                new
                {
                    Id = news.Id,
                    news.Title,
                    news.Content,
                    news.AuthorId,
                    news.PublishDate,
                    news.IsPublished,
                    news.IsDeleted,
                    news.PreviewImage,
                    news.BannerImage
                }, "Default");           
    }

}
