using Contatos.API.Dto;
using Contatos.API.Interfaces;
using Contatos.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutenticacaoController(ITokenService tokenService, ILoggerService loggerService) : ControllerBase
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly ILoggerService _logger = loggerService;

        /// <summary>
        /// Utilize para autenticar o usuário e obter o token JWT
        /// </summary>
        /// <param name="credencial">Credencial de usuário existente</param>
        /// <returns>Token JWT</returns>
        [HttpPost]
        public IActionResult Post([FromBody] CredencialDtoRequest credencial)
        {
            try
            {
                var token = _tokenService.GetToken(credencial.Username, credencial.Password);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    _logger.Info("AutenticacaoController", $"Usuário {credencial.Username} autenticado com sucesso!");
                    return Ok(token);
                }

                _logger.Error("AutenticacaoController", $"Usuário {credencial.Username} não encontrado!");
                return Unauthorized();
            }
            catch(Exception e) 
            {
                _logger.Error("AutenticacaoController", $"Erro ao gerar token de autenticação!\n\n {e}");
                return StatusCode(500);
            }
            
        }
    }
}