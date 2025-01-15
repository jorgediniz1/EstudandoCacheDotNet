using EstudandoCacheDotNet.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace EstudandoCacheDotNet.Services
{
    public class TesteCacheInMemoryService : ITesteCacheInMemory
    {

        private readonly IServiceProvider _serviceProvider;
        private const string VALOR_INSERIR_CACHE = "Variável com valor padrão";

        public TesteCacheInMemoryService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public string RetornaValorCache()
        {
            var cache = _serviceProvider.GetRequiredService<IMemoryCache>();

            MemoryCacheEntryOptions options = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            cache.TryGetValue("ValorCache", out object? valor);

            if(valor is null)
            {
                cache.Set("ValorCache", $"Retornado do Cache {VALOR_INSERIR_CACHE}");
                return VALOR_INSERIR_CACHE;
            }

            return (string)valor;
        }
    }
}
