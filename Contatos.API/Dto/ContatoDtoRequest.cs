using System.ComponentModel.DataAnnotations;
using Contatos.API.Interfaces;
using Contatos.API.Models;

namespace Contatos.API.Dto
{
    public record ContatoDtoRequest: IContatoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O contato deve ser vinculado ao DDD de uma região")]
        public required int DDD { get; set; }

        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
        [Phone(ErrorMessage = "O número de telefone não é válido.")]
        [Required(ErrorMessage = "O número de telefone deve ser informado")]
        public required string Telefone { get; set; }

        [EmailAddress(ErrorMessage = "O email não é válido.")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres.")]
        [Required(ErrorMessage = "O número de telefone deve ser informado")]
        public required string Email { get; set; }

        public static implicit operator Contato(ContatoDtoRequest contatoDtoRequest)
        {
            var contato = new Contato()
            {
                Id = contatoDtoRequest.Id,
                Nome = contatoDtoRequest.Nome,
                DDD = contatoDtoRequest.DDD,
                Telefone = contatoDtoRequest.Telefone,
                Email = contatoDtoRequest.Email,
             };

            return contato;
        }
    }  
}
