using System.Data;
using Contatos.API.Interfaces;
using Contatos.API.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Contatos.API.Repositories
{
    public class ContatoRepository(IDbConnection dbConnection, ICacheService cacheService) : IContatoRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<bool> AlterarContato(Contato contato)
        {
            _cacheService.DeleteKeyMemoryCache("contato");
            return await _dbConnection.UpdateAsync(contato as Contato);
        }

        public async Task<bool> ExcluirContao(int id)
        {
            var contato = _dbConnection.Get<Contato>(id);
            _cacheService.DeleteKeyMemoryCache("contato");
            return await _dbConnection.DeleteAsync(contato);
        }

        public async Task<Contato> InserirNovoContato(Contato contato)
        {
            contato.Id = await _dbConnection.InsertAsync(contato as Contato);
            _cacheService.DeleteKeyMemoryCache("contato");
            return contato;
        }

        public async Task<Tuple<IEnumerable<Contato>, bool>> RetornarListaDeContatos(int? ddd)
        {
            var useCache = true;
            if (!_cacheService.GetDataFromMemoryCache<IEnumerable<Contato>>("contato", out IEnumerable<Contato> listaCacheada))
            {
                var sql = @"SELECT CONTATOS.ID,
                                   CONTATOS.NOME,
                                   CONTATOS.EMAIL, 
                                   CONTATOS.TELEFONE,
                                   CONTATOS.DDD,
                                   REGIOES.DDD,
                                   REGIOES.UF
                              FROM CONTATOS
                        INNER JOIN REGIOES ON CONTATOS.DDD = REGIOES.DDD
                             WHERE 1 = 1";
                ;

                var contatos = await _dbConnection.QueryAsync<Contato, Regiao, Contato>(
                    sql,
                    (contato, regiao) =>
                    {
                        contato.DDD = regiao.DDD;
                        contato.Regiao = regiao;
                        return contato;
                    },
                    splitOn: "DDD"
                );

                _cacheService.CreateKeyMemoryCache<IEnumerable<Contato>>("contato", contatos);
                listaCacheada = contatos;
                useCache = false;
            }
                        
            if (listaCacheada != null && ddd.HasValue)
                listaCacheada = listaCacheada.Where(ctt => ctt.DDD == ddd);

            return new Tuple<IEnumerable<Contato>, bool>(listaCacheada ?? Enumerable.Empty<Contato>(), useCache);
        }

        public async Task<Contato?> RetornarContatoPeloId(int id)
        {
            var contato = await _dbConnection.GetAsync<Contato>(id);
            return contato;
        }
    }
}
