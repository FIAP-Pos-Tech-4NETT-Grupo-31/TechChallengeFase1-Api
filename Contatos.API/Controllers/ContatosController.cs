using System.Net.Mime;
using Contatos.API.Dto;
using Contatos.API.Interfaces;
using Contatos.API.Models;
using Contatos.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Contatos.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ContatosController(IContatoService contatoService) : ControllerBase
    {
        private readonly IContatoService _contatoService = contatoService;

        /// <summary>
        /// Retorna a lista de todos contatos cadastrados
        /// </summary>
        /// <param name="ddd">(Opcional) Informar para retornar contatos filtrados por DDD</param>
        /// <returns name="ContatoDto"></returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<ContatoDtoResponse>>(200)]
        public async Task<IActionResult> GetAll([FromQuery] int? ddd)
        {
            var contatos = await _contatoService.RetornarListaDeContatos(ddd);
            var contatosDto = contatos.Select(contato => (ContatoDtoResponse)contato);
            return Ok(contatosDto);
        }

        /// <summary>
        /// Retorna um contato através do seu ID
        /// </summary>
        /// <param name="id">Código do contato</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType<ContatoDtoResponse>(200)]
        public async Task<IActionResult> GetById(int id)
        {
            var contato = await _contatoService.RetornarContatoPeloId(id);
            return contato is not null ? Ok((ContatoDtoResponse)contato) : NotFound();
        }

        /// <summary>
        /// Insere um novo contato
        /// </summary>
        /// <param name="contatoDto">Dados do contato</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<ContatoDtoResponse>(200)]
        public async Task<IActionResult> Post([FromBody] ContatoDtoRequest contatoDto)
        {
            var contato = (Contato)contatoDto;
            var novoContato = await _contatoService.InserirNovoContato(contato);
            return Created(string.Empty, novoContato);     
        }

        /// <summary>
        /// Altera os dados de um contato existente
        /// </summary>
        /// <param name="id">Código do contato</param>
        /// <param name="contatoDtoRequest">Dados do contato para alteração</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] ContatoDtoRequest contatoDtoRequest)
        {
            var contato = (Contato)contatoDtoRequest;
            contato.Id = id;
            var atualizado = await _contatoService.AlterarContato(contato);
            return atualizado ? NoContent() : NotFound();
        }

        /// <summary>
        /// Exclui um contato existente
        /// </summary>
        /// <param name="id">Id do contato para exclusão</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var contato = await _contatoService.RetornarContatoPeloId(id);            

            if (contato is null)
            {
                return NotFound();
            }

            await _contatoService.ExcluirContato(id);
            return NoContent();
        }
    }
}
