using HackerNews.Models;

namespace HackerNews.Service
{
    public interface IHackerNewsService
    {
        Task<List<StoryResponseModel>> GetBestStoriesAsync(int numberOfStories);
        Task<StoryResponseModel> GetBestStoryAsync(int storyId);
    }
}
