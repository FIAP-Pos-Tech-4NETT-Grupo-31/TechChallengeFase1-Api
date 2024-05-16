using Contatos.API.Dto;
using Contatos.API.Interfaces;
using Contatos.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contatos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController(ITokenService tokenService) : ControllerBase
    {
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Utilize para autenticar o usuário e obter o token JWT
        /// </summary>
        /// <param name="credencial">Credencial de usuário existente</param>
        /// <returns>Token JWT</returns>
        [HttpPost]
        public IActionResult Post([FromBody] CredencialDtoRequest credencial)
        {
            var token = _tokenService.GetToken(credencial.Username, credencial.Password);

            if (!string.IsNullOrWhiteSpace(token))
            {
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}
