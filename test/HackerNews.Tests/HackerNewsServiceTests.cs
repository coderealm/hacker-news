using HackerNews.Http;
using HackerNews.Serialize;
using HackerNews.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HackerNews.Tests
{
    public class HackerNewsServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private Mock<IMemoryCache> _mockCache;
        private Mock<ISerializer> _serializerMock;
        private Mock<IHttpService> _httpServiceMock;
        private string BestStoriesIdsApiUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
        private string storyIds = "[38971012,38981254,38985152,38983067,38971221,38992601]";

        public HackerNewsServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _mockCache = new Mock<IMemoryCache>();
            _serializerMock = new Mock<ISerializer>();
            _httpServiceMock = new Mock<IHttpService>();
        }

        [Fact]
        public async Task HackerNewsService_GetBestStoriesAsync_Returns_Stories()
        {
            _configurationMock.Setup(x => x["HackerNews:BestStoriesIdsApi"]).Returns(BestStoriesIdsApiUrl);
            var response = Task.Factory.StartNew<string>(() => storyIds);
            _httpServiceMock.Setup(x => x.GetAsync(BestStoriesIdsApiUrl)).Returns(() => response);
            var expected = new List<int> { 38971012, 38981254, 38985152, 38983067, 38971221, 38992601 };
            _serializerMock.Setup(x => x.Deserialize<List<int>>(storyIds)).Returns(() => expected);
            var hackerNewsService = new HackerNewsService(_configurationMock.Object, _mockCache.Object, _serializerMock.Object, _httpServiceMock.Object);
            var bestStories = await hackerNewsService.GetStoryIdsAsync(6);
            Assert.Equal(expected, bestStories);
        }
    }
}
