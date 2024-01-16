using Newtonsoft.Json;

namespace HackerNews.Serialize
{
    public class Serializer: ISerializer
    {
        public T Deserialize<T>(string input) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(input, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }) ?? new T();
        }
    }
}
