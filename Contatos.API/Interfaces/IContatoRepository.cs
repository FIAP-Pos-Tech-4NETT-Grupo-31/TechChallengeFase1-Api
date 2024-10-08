﻿using Contatos.API.Models;

namespace Contatos.API.Interfaces
{
    public interface IContatoRepository
    {
        Task<Tuple<IEnumerable<Contato>, bool>> RetornarListaDeContatos(int? ddd = null);

        Task<Contato?> RetornarContatoPeloId(int id);

        Task<Contato> InserirNovoContato(Contato contato);

        Task<bool> ExcluirContao(int id);

        Task<bool> AlterarContato(Contato contato);
    }
}
