using System.Data;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace Orange.ApiTokenValidation.Repositories.DapperConfig
{
    class JObjectHandler : SqlMapper.TypeHandler<JObject>
    {
        public override JObject Parse(object value)
        {
            var json = (string)value;

            return json == null ? null : JObject.Parse(json);
        }

        public override void SetValue(IDbDataParameter parameter, JObject value)
        {
            
            parameter.Value = value?.ToString(Formatting.None);

            ((NpgsqlParameter)parameter).NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
        }
    }
}
