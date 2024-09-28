using Contatos.API.Interfaces;
using Contatos.API.Models;

namespace Contatos.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public UsuarioRepository()
        {
            ListaUsuario.Usuarios = new List<Usuario>();
            ListaUsuario.Usuarios.Add(new Usuario { Id = 1, Username = "Grupo31", Password = "Grupo#31" });
            ListaUsuario.Usuarios.Add(new Usuario { Id = 2, Username = "Userteste", Password = "Senha@123&Teste@!" });
        }

        public Usuario? RetornarUsuario(string username, string password)
        {
            return ListaUsuario.Usuarios
                .Where(usuario => usuario.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && usuario.Password == password)
                .FirstOrDefault();
        }
    }
}