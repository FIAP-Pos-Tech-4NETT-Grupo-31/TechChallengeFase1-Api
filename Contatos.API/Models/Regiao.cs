using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contatos.API.Dto;

namespace Contatos.API.Models
{
    [Table("REGIOES")]
    public class Regiao
    {
        [Dapper.Contrib.Extensions.Key]
        [Required(ErrorMessage = "O código DDD é obrigatório.")]
        public required int DDD { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        public required string UF { get; set; }

        public static implicit operator RegiaoDtoResponse(Regiao regiao)
        {
            var regiaoDtoResponse = new RegiaoDtoResponse()
            {
                DDD = regiao.DDD,
                UF = regiao.UF,
            };

            return regiaoDtoResponse;
        }
    }
}
