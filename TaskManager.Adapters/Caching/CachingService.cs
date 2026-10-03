using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using TaskManager.Core.Ports.Caching;

namespace TaskManager.Adapters.Caching;

public class CachingService : ICachingPort
{

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    private readonly IDistributedCache _cache;
    private readonly ILogger<CachingService> _logger;

    public CachingService(IDistributedCache cache, ILogger<CachingService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await _cache.GetStringAsync(key);
            return string.IsNullOrEmpty(value) ? default : JsonSerializer.Deserialize<T>(value, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao ler o cache (chave {Key}).", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
                options.AbsoluteExpirationRelativeToNow = expiration;

            await _cache.SetStringAsync(key, JsonSerializer.Serialize(value, JsonOptions), options);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao gravar no cache (chave {Key}).", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao invalidar o cache (chave {Key}); dado pode ficar defasado até o TTL.", key);
        }
    }
}