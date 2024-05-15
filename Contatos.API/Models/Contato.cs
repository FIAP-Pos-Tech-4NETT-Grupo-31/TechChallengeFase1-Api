using System.ComponentModel.DataAnnotations;
using Contatos.API.Dto;
using Dapper.Contrib.Extensions;

namespace Contatos.API.Models
{
    [Table("Contatos")]
    public record Contato
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        public required string Nome { get; set; }
       
        public required int DDD { get; set; }

        public required string Telefone { get; set; }

        public required string Email { get; set; }
       
        [Write(false)]
        public RegiaoDtoResponse? Regiao { get; set; }

        public static implicit operator ContatoDtoResponse(Contato contato)
        {
            var contatoDto = new ContatoDtoResponse()
            {
                Id = contato.Id,
                Nome = contato.Nome,
                DDD = contato.DDD,
                Telefone = contato.Telefone,
                Email = contato.Email,
                Regiao = contato.Regiao
            };

            return contatoDto;
        }
    }
}
