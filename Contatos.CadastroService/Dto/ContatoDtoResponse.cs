﻿using Contatos.CadastroService.Interfaces;

namespace Contatos.InclusaoService.Dto
{
    public record ContatoDtoResponse: IContatoDto
    {
        public int Id { get; set; }

        public required string Nome { get; set; }

        public required int DDD { get; set; }

        public required string Telefone { get; set; }

        public required string Email { get; set; }        
    }  
}
