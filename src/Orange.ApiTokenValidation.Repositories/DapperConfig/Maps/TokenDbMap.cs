using Dapper.Dommel.FluentMapping;
using Orange.ApiTokenValidation.Repositories.Models;

namespace Orange.ApiTokenValidation.Repositories.DapperConfig.Maps
{
    public class TokenDbMap : DommelEntityMap<TokenDbModel>
    {
        //IsKey - this property used by Dommel for control insert or update functions.
        //This orm can use multi column key. We can use natural Id key (concatenation two or more columns)
        //or create some unique key in column, but this key will end up in the domain, this not good.
        public TokenDbMap()
        {
            ToTable("tokentable");
            Map(x => x.Issuer).ToColumn("issuer").IsKey();
            Map(x => x.Audience).ToColumn("audience");
            Map(x => x.PrivateKey).ToColumn("private_key");
            Map(x => x.Ttl).ToColumn("ttl");
            Map(x => x.ExpirationDate).ToColumn("expiration_time");
            Map(x => x.IsActive).ToColumn("is_active");
            Map(x => x.PayLoad).ToColumn("payload");
            Map(x => x.CreatedTime).ToColumn("created_time");
            Map(x => x.Creator).ToColumn("creator");
            Map(x => x.UpdatedTime).ToColumn("updated_time");
            Map(x => x.Updater).ToColumn("updater");
        }
    }
}
