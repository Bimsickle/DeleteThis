using BookRadarLibrary.Models;

namespace BookRadarLibrary.DataAccess
{
    public interface INewsArticleData
    {
        Task CreateNewsArticle(NewsArticleModel model);
        Task<IEnumerable<NewsArticleModel>> GetNewsArticles();
        Task<IEnumerable<NewsArticleModel>> GetNewsArticlesAdmin();
        Task<NewsArticleModel> GetOneNewsArticle(Guid id);
        Task UpdateNewsArticle(NewsArticleModel news);
    }
}