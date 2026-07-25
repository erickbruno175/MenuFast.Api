using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json.Serialization;

namespace MenuFast.Api.Api.Application.Services.Redis;

public class RedisService {
    private readonly IDatabase _database;
    public RedisService(IConnectionMultiplexer redis) {
        _database = redis.GetDatabase();
    }


    public async Task SetAsync<T>(string chave,T valor,TimeSpan? expiracao = null) {var json = JsonConvert.SerializeObject(valor);

        await _database.StringSetAsync(chave,json,(Expiration)expiracao);
    }

    public async Task<T?> GetAsync<T>(string chave) {
var resultado = await _database.StringGetAsync(chave);

        if(resultado.IsNullOrEmpty)
            return default;

        return JsonConvert.DeserializeObject<T>(resultado!);
    }

    public async Task RemoveAsync(string chave) {
        await _database.KeyDeleteAsync(chave);
    }

    public async Task<bool> ExistsAsync(string chave) {
        return await _database.KeyExistsAsync(chave);
    }
}