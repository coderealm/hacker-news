namespace HackerNews.Http
{

    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("HackerNewsClient");
        }
        public async Task<string> GetAsync(string url)
        {
            var httpResponseMessage = await _httpClient.GetAsync(url);
            if (httpResponseMessage == null || !httpResponseMessage.IsSuccessStatusCode)
            {
                return string.Empty;
            }
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
