using HackerNews.Http;
using HackerNews.Models;
using HackerNews.Serialize;
using HackerNews.Service;
using Microsoft.Extensions.Configuration;
using Moq;

namespace HackerNews.Tests
{
    public class HackerNewsServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private Mock<ISerializer> _serializerMock;
        private Mock<IHttpService> _httpServiceMock;
        private string BestStoriesIdsApiUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
        private string BestStoryApiUrl = "https://hacker-news.firebaseio.com/v0/item/{0}.json";
        private string storyIds = "[38971012,38981254,38985152,38983067,38971221,38992601]";
        private string BestStories = "{\"by\":\"edward\",\"descendants\":730,\"id\":38971012,\"kids\":[38974341,38976554,38971764,38976185,38971467,38975923,38973086,38974924,38972167,38971740,38971527,38971438,38973385,38971653,38971383,38976444,38971900,38971507,38972365,38974477,38972525,38972817,38977107,38971481,38974225,38972670,38971840,38973572,38974511,38973106,38972684,38971469,38976820,38973851,38972792,38990748,38980602,38971728,38974448,38973813,38972105,38972428,38972537,38976671,38976897,38972993,38987965,38974007,38972475,38972176,38972129,38974935,38974461,38977070,38972578,38971725,38971732,38971672,38972043,38973031,38976661,38974893,38971419,38973847,38972715,38978215,38973737,38978198,38977109,38972583,38973037,38977510,38972574,38975895,38977438,38973739,38973378,38971069,38974617,38971609,38976490,38972546,38977215,38976754,38972420,38973313,38976881,38972191,38973103,38972202,38972916,38971710,38971910,38972095,38976798,38972030,38977194,38973041,38971834],\"score\":1319,\"time\":1705080430,\"title\":\"I'm sorry but I cannot fulfill this request it goes against OpenAI use policy\",\"type\":\"story\",\"url\":\"https://www.amazon.com/fulfill-request-respectful-information-users-Brown/dp/B0CM82FJL2\"}";

        public HackerNewsServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
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
            var hackerNewsService = new HackerNewsService(_configurationMock.Object, _serializerMock.Object, _httpServiceMock.Object);
            var bestStories = await hackerNewsService.GetStoryIdsAsync(6);
            Assert.Equal(expected, bestStories);
        }

        [Fact]
        public async Task HackerNewsService_GetBestStoryAsync_Returns_Story()
        {
            var expected = new StoryResponseModel
            {
                By = "edward",
                Score = 1319,
                Title = "I'm sorry but I cannot fulfill this request it goes against OpenAI use policy",
                Type = "story",
                Url = "https://www.amazon.com/fulfill-request-respectful-information-users-Brown/dp/B0CM82FJL2"
            };
            _configurationMock.Setup(x => x["HackerNews:BestStoryApi"]).Returns(BestStoryApiUrl);
            var response = Task.Factory.StartNew<string>(() => BestStories);
            _httpServiceMock.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(() => response);

            _serializerMock.Setup(x => x.Deserialize<StoryResponseModel>(BestStories)).Returns(() => expected);
            var hackerNewsService = new HackerNewsService(_configurationMock.Object, _serializerMock.Object, _httpServiceMock.Object);
            var bestStory = await hackerNewsService.GetBestStoryAsync(38971012);
            Assert.Equal(expected, bestStory);
        }
    }
}
