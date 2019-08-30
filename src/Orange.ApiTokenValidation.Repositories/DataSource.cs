using System.Data;
using Dapper;
using Dapper.FluentMap;
using Newtonsoft.Json.Linq;
using Npgsql;
using Orange.ApiTokenValidation.Repositories.Maps;

namespace Orange.ApiTokenValidation.Repositories
{
    public class DataSource: IDataSource
    {
        static DataSource()
        {
            SqlMapper.AddTypeHandler<JObject>(new JObjectHandler());

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new TokenDbMap());
            });
        }

        private readonly DataSourceConfiguration _configuration;

        public DataSource(DataSourceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection Connection => new NpgsqlConnection(_configuration.ConnectionString);
    }
}
