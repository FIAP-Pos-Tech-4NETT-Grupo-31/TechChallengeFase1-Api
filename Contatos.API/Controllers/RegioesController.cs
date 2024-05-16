using Contatos.API.Dto;
using Contatos.API.Interfaces;
using Contatos.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RegioesController(IRegiaoService regiaoService) : ControllerBase
    {
        private readonly IRegiaoService _regiaoService = regiaoService;

        /// <summary>
        /// Retorna a lista de regiões/DDD's
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<RegiaoDtoResponse>>(200)]
        public async Task<IActionResult> GetAll()
        {
            var regioes = await _regiaoService.RetornarListaDeRegioes();
            var regioesDto = regioes.Select(regiao => (RegiaoDtoResponse)regiao);
            return Ok(regioesDto);
        }
    }
}