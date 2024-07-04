using System.Data;
using System.Data.Common;

namespace Contatos.API.IntegrationTests
{
    public class DatabaseInitializer
    {
        public static async Task InitializeDatabaseAsync(DbConnection connection)
        {
            await CreateContatosTableAsync(connection);
            await CreateRegioesTableAsync(connection);
        }

        private static async Task CreateContatosTableAsync(DbConnection connection)
        {
            var createContatosTableCmd = @"
            CREATE TABLE IF NOT EXISTS [CONTATOS] (
                [Id]       INTEGER PRIMARY KEY AUTOINCREMENT,
                [Nome]     NVARCHAR (100) NOT NULL,
                [Email]    VARCHAR (200)  NOT NULL,
                [Telefone] NVARCHAR (20)  NOT NULL,
                [DDD]      INT            NOT NULL
            );";

            using (var command = connection.CreateCommand())
            {
                command.CommandText = createContatosTableCmd;
                await command.ExecuteNonQueryAsync();
            }
        }

        private static async Task CreateRegioesTableAsync(DbConnection connection)
        {
            var createRegioesTableCmd = @"
            CREATE TABLE IF NOT EXISTS [REGIOES] (
                [DDD] INTEGER PRIMARY KEY AUTOINCREMENT,
                [UF]  TEXT NOT NULL
            );";

            using (var command = connection.CreateCommand())
            {
                command.CommandText = createRegioesTableCmd;
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}