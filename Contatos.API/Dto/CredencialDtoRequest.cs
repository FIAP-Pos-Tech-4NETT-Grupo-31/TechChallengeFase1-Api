using System.ComponentModel.DataAnnotations;

namespace Contatos.API.Dto
{
    public class CredencialDtoRequest
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required string Password { get; set; }
    }
}
