using Contatos.API.Models;

namespace Contatos.API.Interfaces
{
    public interface ITokenService
    {
        public string GetToken(string username, string password);
    }
}
