using System.Diagnostics;
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
    public class ContatosController(IContatoService contatoService, ILoggerService? loggerService) : ControllerBase
    {
        private readonly IContatoService _contatoService = contatoService;
        private readonly ILoggerService? _logger = loggerService;

        /// <summary>
        /// Retorna a lista de todos contatos cadastrados
        /// </summary>
        /// <param name="ddd">(Opcional) Informar para retornar contatos filtrados por DDD</param>
        /// <returns name="ContatoDto"></returns>
        [HttpGet]
        [ProducesResponseType<IEnumerable<ContatoDtoResponse>>(200)]
        public async Task<IActionResult> GetAll([FromQuery] int? ddd)
        {
            try
            {
                var sw = Stopwatch.StartNew();

                var contatos = await _contatoService.RetornarListaDeContatos(ddd);
                var contatosDto = contatos.Item1.Select(contato => (ContatoDtoResponse)contato);

                _logger?.Info("ContatosController", $"Os contatos foram buscadas no banco de dados com sucesso. A cache de memória {(contatos.Item2 ? "" : "não ")}foi usada!", sw);
                return Ok(contatosDto);
            }
            catch(Exception e)
            {
                _logger?.Error("ContatosController", $"Erro ao acessar contatos!\n\n {e}");
                return StatusCode(500);
            }
            
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
            try
            {
                var sw = Stopwatch.StartNew();
                var contato = await _contatoService.RetornarContatoPeloId(id);

                _logger?.Info("ContatosController", $"O contato de id = '{id}' foi buscado no banco de dados com sucesso.", sw);
                return contato is not null ? Ok((ContatoDtoResponse)contato) : NotFound();
            }
            catch (Exception e)
            {
                _logger?.Error("ContatosController", $"Erro ao acessar contato por id '{id}'!\n\n {e}");
                return StatusCode(500);
            }

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
            try
            {
                var sw = Stopwatch.StartNew();
                var contato = (Contato)contatoDto;
                var novoContato = await _contatoService.InserirNovoContato(contato);

                _logger?.Info("ContatosController", $"O contato de {contatoDto.Nome} foi adicionado no banco de dados com sucesso.", sw);
                return Created(string.Empty, novoContato);
            }
            catch (Exception e)
            {
                _logger?.Error("ContatosController", $"Erro ao adicionar contato no banco!\n\n {e}");
                return StatusCode(500);
            }
                
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
            try
            {
                var sw = Stopwatch.StartNew();
                var contato = (Contato)contatoDtoRequest;
                contato.Id = id;
                var atualizado = await _contatoService.AlterarContato(contato);

                _logger?.Info("ContatosController", $"O contato de {contatoDtoRequest.Nome} foi atualizado no banco de dados com sucesso.", sw);
                return atualizado ? NoContent() : NotFound();
            }catch (Exception e)
            {
                _logger?.Error("ContatosController", $"Erro ao atualizar contato '{contatoDtoRequest.Nome}' no banco!\n\n {e}");
                return StatusCode(500);
            }
            
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
            try
            {
                var sw = Stopwatch.StartNew();
                var contato = await _contatoService.RetornarContatoPeloId(id);

                if (contato is null)
                {
                    _logger?.Info("ContatosController", $"O contato '{id}' não foi encontrado no banco de dados.", sw);
                    return NotFound();
                }

                await _contatoService.ExcluirContato(id);

                _logger?.Info("ContatosController", $"O contato '{id}' foi excluído do banco de dados com sucesso.", sw);
                return NoContent();
            }catch(Exception e)
            {
                _logger?.Error("ContatosController", $"Erro ao excluir contato '{id}' no banco!\n\n {e}");
                return StatusCode(500);
            }
        }
    }
}
