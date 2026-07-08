namespace TaskManager.Adapters.Caching
{
    public interface ICachingPort
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<T?> GetAsync<T>(string key);
    }
}
