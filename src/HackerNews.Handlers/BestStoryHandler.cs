using HackerNews.Models;
using MediatR;
using HackerNews.Service;

namespace HackerNews.Handlers
{
    public class BestStoryHandler : IRequestHandler<StoryRequestModel, List<StoryResponseModel>>
    {
        private readonly IHackerNewsService _hackerNewsService;
        public BestStoryHandler(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }
        public async Task<List<StoryResponseModel>> Handle(StoryRequestModel storyRequestModel, CancellationToken cancellationToken)
        {
            return await _hackerNewsService.GetBestStoriesAsync(storyRequestModel.NumberOfStories);
        }
    }
}
