using HackerNews.Models;

namespace HackerNews.Http
{
    public interface IHttpService
    {
        Task<string> GetAsync(string url);
    }
}
