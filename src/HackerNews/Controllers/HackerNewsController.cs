using HackerNews.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    [Route("api/hacker-news")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private IStoryRequestModel _storyRequestModel;
        public HackerNewsController(IMediator mediator, IStoryRequestModel storyRequestModel)
        {
            _mediator = mediator;
            _storyRequestModel = storyRequestModel;

        }

        [HttpGet, Route("best-stories")]
        public async Task<IActionResult> GetBestStoriesAsync(int bestStories)
        {
                _storyRequestModel.NumberOfStories = bestStories;
                var bestStoryModels = await _mediator.Send(_storyRequestModel);
                return Ok(bestStoryModels);
        }
    }
}
