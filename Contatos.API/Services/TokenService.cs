using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Contatos.API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Contatos.API.Services
{
    public class TokenService(IConfiguration configuration, IUsuarioRepository usuarioRepository): ITokenService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public string GetToken(string username, string password)
        {
            var usuario = _usuarioRepository.RetornarUsuario(username, password);

            if (usuario is null)
            {
                return string.Empty;
            }

            // Variável responsável por gerar o token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Recupera a chave que criamos no nosso appSettings e convert para um array de bytes
            var chaveCriptografia = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJWT"));

            // O Descriptor é responsável por definir todas as propriedades que o nosso token terá quando descriptografarmos
            var tokenPropriedades = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Username),
                    //new Claim(ClaimTypes.Role, (usuario.PermissaoSistema - 1).ToString()),
                    //new Claim("ClaimPersonalizada_1", "Nossa claim 1"),
                    //new Claim("ClaimPersonalizada_2", "Nossa claim 2")
                }),

                // Tempo de expiração do token
                Expires = DateTime.UtcNow.AddHours(8),

                // Adiciona a nossa chave de criptografia
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(chaveCriptografia),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Cria o nosso token e devolve pro método solicitante
            var token = tokenHandler.CreateToken(tokenPropriedades);
            return tokenHandler.WriteToken(token);

        }
    }
}
