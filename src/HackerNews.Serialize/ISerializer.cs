namespace HackerNews.Serialize
{
    public interface ISerializer
    {
        T Deserialize<T>(string input) where T : class, new();
    }
}
