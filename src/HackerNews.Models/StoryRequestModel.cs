using MediatR;

namespace HackerNews.Models
{
    public class StoryRequestModel : IRequest<List<StoryResponseModel>>, IStoryRequestModel
    {
        public int NumberOfStories { get; set; }
    }
}
