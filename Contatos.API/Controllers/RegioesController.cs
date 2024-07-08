using Contatos.API.Dto;
using Contatos.API.Interfaces;
using Contatos.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;

namespace Contatos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class RegioesController(IRegiaoService regiaoService, ILoggerService? loggerService) : ControllerBase
    {
        private readonly IRegiaoService _regiaoService = regiaoService;
        private readonly ILoggerService? _logger = loggerService;

        /// <summary>
        /// Retorna a lista de regiões/DDD's
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<RegiaoDtoResponse>>(200)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var sw = Stopwatch.StartNew();

                var regioes = await _regiaoService.RetornarListaDeRegioes();
                var regioesDto = regioes.Item1.Select(regiao => (RegiaoDtoResponse)regiao);
                _logger?.Info("RegioesController", $"As regiões foram buscadas no banco de dados com sucesso. A cache de memória {(regioes.Item2 ? "não " : "")}foi usada!", sw);

                return Ok(regioesDto);
            } 
            catch(Exception e)
            {
                _logger?.Error("RegioesController", $"Erro ao acessar regiões!\n\n {e}");
                return StatusCode(500);
            }
            
        }
    }
}