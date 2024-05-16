namespace Contatos.API.Models
{
    // Lista que vamos usar em memória para simular a consulta de usuários cadastrados
    public static class ListaUsuario
    {
        public static IList<Usuario> Usuarios { get; set; }
    }

    public class Usuario
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
