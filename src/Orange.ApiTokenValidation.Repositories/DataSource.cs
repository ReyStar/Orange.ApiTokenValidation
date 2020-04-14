using System;
using System.Data;
using Dapper;
using Dapper.Dommel;
using Dapper.Dommel.FluentMapping;
using Dapper.FluentMap;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Npgsql;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Repositories.DapperConfig;
using Orange.ApiTokenValidation.Repositories.DapperConfig.Maps;

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
                config.ApplyToDommel();
            });
            
            DommelMapper.SetPropertyResolver(new DommelPropertyResolverForCustomTypes());
        }

        private readonly ConnectionStrings _configuration;

        public DataSource(IOptions<ConnectionStrings> configuration, IOptions<CommonConfiguration> commonConfiguration)
        {
            _configuration = configuration.Value;

            InstanceId = !string.IsNullOrWhiteSpace(commonConfiguration.Value.InstanceId)
                ? commonConfiguration.Value.InstanceId
                : throw new ArgumentNullException($"Argument {nameof(commonConfiguration.Value.InstanceId)} can't be null or empty");
        }

        public string InstanceId { get; }

        public IDbConnection Connection => new NpgsqlConnection(_configuration.DefaultConnection);
    }
}
