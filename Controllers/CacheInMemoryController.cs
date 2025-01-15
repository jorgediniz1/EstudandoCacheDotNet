using EstudandoCacheDotNet.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EstudandoCacheDotNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheInMemoryController : ControllerBase
    {
        private readonly ITesteCacheInMemory _testeCache;
        public CacheInMemoryController(ITesteCacheInMemory testeCache)
        {
            _testeCache = testeCache;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var resultado = _testeCache.RetornaValorCache();
            return Ok(resultado);
        }
    }
}
