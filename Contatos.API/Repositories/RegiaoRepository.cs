using System.Data;
using Contatos.API.Interfaces;
using Contatos.API.Models;
using Contatos.API.Services;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Contatos.API.Repositories
{
    public class RegiaoRepository(IDbConnection dbConnection, ICacheService cacheService) : IRegiaoRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Tuple<IEnumerable<Regiao>, bool>> RetornarListaDeRegioes()
        {
            if (!_cacheService.GetDataFromMemoryCache<IEnumerable<Regiao>>("regiao", out IEnumerable<Regiao> listaCacheada))
            {
                var regioes = await _dbConnection.GetAllAsync<Regiao>();

                _cacheService.CreateKeyMemoryCache("regiao", regioes);

                return new Tuple<IEnumerable<Regiao>, bool>(regioes, true);
            }

            return new Tuple<IEnumerable<Regiao>, bool>(listaCacheada ?? Enumerable.Empty<Regiao>(), false);
        }
    }
}
