using Contatos.API.Models;

namespace Contatos.API.Interfaces
{
    public interface IUsuarioRepository
    {
        Usuario? RetornarUsuario(string username, string password);
    }
}
