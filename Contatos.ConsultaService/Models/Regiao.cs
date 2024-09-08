using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ConsultaService.Models
{
    [Keyless]
    public class Regiao
    {
        [Required(ErrorMessage = "O código DDD é obrigatório.")]
        public required int DDD { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório.")]
        public required string UF { get; set; }
    }
}
