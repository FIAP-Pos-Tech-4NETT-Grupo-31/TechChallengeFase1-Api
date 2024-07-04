using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Contatos.API.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly DbConnection _connection;

        public CustomWebApplicationFactory()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();            
        }

        protected async override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remova a configuração existente de IDbConnection
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDbConnection));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Substitua a configuração do serviço de banco de dados para usar a conexão SQLite em memória
                services.AddSingleton<IDbConnection>(provider => _connection);
            });

            await DatabaseInitializer.InitializeDatabaseAsync(_connection);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _connection?.Dispose();
            }
        }
    }
}