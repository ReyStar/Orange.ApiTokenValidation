using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Orange.ApiTokenValidation.Common;

namespace Orange.ApiTokenValidation.DataBaseProducer
{
    class DataBaseCreator
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public DataBaseCreator(IConfigurationRoot configurationRoot, ILogger<DataBaseCreator> logger)
        {
            _logger = logger;
            _connectionString =  configurationRoot.GetConnectionString("DefaultConnection"); 
        }

        public async Task RunAsync(CancellationToken token = default)
        {
            var connectionStringBuilder =  new NpgsqlConnectionStringBuilder(_connectionString);
            var dataBase = connectionStringBuilder.Database;
            connectionStringBuilder.Database = null;

            await using var connection = new NpgsqlConnection(connectionStringBuilder.ToString());

            var isExist =
                await connection.ExecuteScalarAsync<bool?>(new CommandDefinition(
                    $"SELECT (datname IS NOT NULL) FROM pg_database WHERE datname='{dataBase}'",
                    cancellationToken: token));

            if (!isExist.HasValue
                || !isExist.Value)
            {
                var resourceLoader = new ResourceLoader(typeof(DataBaseCreator).Assembly);
                var dbCreationScript = await resourceLoader.LoadStringAsync($"{typeof(DataBaseCreator).Namespace}.Scripts.CreateTokenDB.sql");
                await connection.QueryAsync(new CommandDefinition(
                    string.Format(dbCreationScript, dataBase, connectionStringBuilder.Username),
                    cancellationToken: token));
                    
                _logger.LogInformation($"{dataBase} was created");
            }
                
            await connection.CloseAsync();
        }
    }
}
