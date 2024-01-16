﻿using HackerNews.Http;
using HackerNews.Models;
using HackerNews.Serialize;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace HackerNews.Service
{
    public class HackerNewsService: IHackerNewsService
    {
        private readonly IConfiguration _configuration;
        private IMemoryCache _cache;
        private ISerializer _serializer;
        private IHttpService _httpService;

        public HackerNewsService(IConfiguration configuration, IMemoryCache cache, 
            ISerializer serializer, IHttpService httpService)
        {
            _configuration = configuration;
            _cache = cache;
            _serializer = serializer;
            _httpService = httpService;
        }

        public async Task<List<StoryResponseModel>> GetBestStoriesAsync(int numberOfStories)
        {
            var bestStoriesIdsApiUrl = _configuration["HackerNews:BestStoriesIdsApi"];
            if (string.IsNullOrWhiteSpace(bestStoriesIdsApiUrl))
            {
                 throw new ArgumentNullException($"HackerNews:BestStoriesIdsApi url is empty string: {nameof(bestStoriesIdsApiUrl)}");
            }
            var httpResponseMessage = await _httpService.GetAsync(bestStoriesIdsApiUrl);
            var storyIds = _serializer.Deserialize<List<int>>(httpResponseMessage);
            var storyResponseModel = storyIds?.Take(numberOfStories)?.Select(GetBestStoryAsync);

            var storyResponseModels = new List<StoryResponseModel>();
            if (storyResponseModel == null)
            {
                return storyResponseModels;
            }
            storyResponseModels = (await Task.WhenAll(storyResponseModel)).OrderByDescending(x => x.Score).ToList();
            return storyResponseModels;
        }

        public async Task<StoryResponseModel> GetBestStoryAsync(int storyId)
        {
            var bestStory = await _cache.GetOrCreateAsync(storyId, async cacheEntry =>
            {
                var storyResponseModel = new StoryResponseModel();
                var apiUrl = _configuration["HackerNews:BestStoryApi"];
                if (string.IsNullOrWhiteSpace(apiUrl))
                {
                    throw new ArgumentNullException($"HackerNews:BestStoryApi url is empty string: {nameof(apiUrl)}");
                }
                var bestStoryApi = string.Format(apiUrl, storyId);
                var httpResponseMessage = await _httpService.GetAsync(bestStoryApi);
                return _serializer.Deserialize<StoryResponseModel>(httpResponseMessage);
            });

            return bestStory ?? new StoryResponseModel();
        }
    }
}