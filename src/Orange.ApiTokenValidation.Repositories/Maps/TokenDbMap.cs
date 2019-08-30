using Dapper.FluentMap.Mapping;
using Orange.ApiTokenValidation.Repositories.Models;

namespace Orange.ApiTokenValidation.Repositories.Maps
{
    public class TokenDbMap : EntityMap<TokenDbModel>
    {
        public TokenDbMap()
        {
            Map(x => x.Issuer).ToColumn("issuer");
            Map(x => x.Audience).ToColumn("audience");
            Map(x => x.PrivateKey).ToColumn("private_key");
            Map(x => x.Ttl).ToColumn("ttl");
            Map(x => x.ExpirationDate).ToColumn("expiration_time");
            Map(x => x.IsActive).ToColumn("is_active");
            Map(x => x.PayLoad).ToColumn("payload");
        }
    }
}
