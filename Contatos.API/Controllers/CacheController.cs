using Contatos.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Contatos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CacheController (ICacheService cacheService, ILoggerService logger) : ControllerBase
    {   
        private readonly ICacheService _cacheService = cacheService;
        private readonly ILoggerService _logger = logger;

        /// <summary>
        /// Limpa toda a cache de memória da API
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            try
            {
                _cacheService.ClearMemoryCache();
                _logger.Info("CacheController", "Cache de memória da API excluída!");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error("CacheController", $"Erro ao excluir cache de memória da API!\n\n {e}");
                return StatusCode(500);
            }
            
        }

        /// <summary>
        /// Limpa uma chave da cache de memória da API
        /// </summary>
        /// <param name="key">Chave da cache de memória para deletar</param>
        /// <returns></returns>
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteByKey(string key)
        {
            try
            {
                _cacheService.DeleteKeyMemoryCache(key);
                _logger.Info("CacheController", $"A chave {key} foi excluída da cache de memória da API!");
                return Ok();
            }
            catch (Exception e) 
            {
                _logger.Error("CacheController", $"Erro ao excluir chave {key} da cache de memória da API!\n\n {e}");
                return StatusCode(500);
            }
            
        }

    }
}
