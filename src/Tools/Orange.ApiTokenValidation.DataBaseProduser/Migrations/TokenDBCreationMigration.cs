using FluentMigrator;

namespace Orange.ApiTokenValidation.DataBaseProducer.Migrations
{
    //https://fluentmigrator.github.io/articles/quickstart.html?tabs=runner-in-process
    [Migration(1, "Create token table")]
    public class TokenDBCreationMigration: ForwardOnlyMigration
    {
        private const string TableName = "tokentable";

        public override void Up()
        {
            Create
                .Table(TableName)
                .WithColumn("issuer").AsString(255).NotNullable()
                .WithColumn("audience").AsString(255).NotNullable()
                .WithColumn("private_key").AsString(255).NotNullable()
                .WithColumn("ttl").AsInt32().NotNullable()
                .WithColumn("expiration_time").AsDateTimeOffset().NotNullable()
                .WithColumn("is_active").AsBoolean().NotNullable()
                .WithColumn("payload").AsCustom("jsonb").Nullable()
                .WithColumn("created_time").AsDateTimeOffset().NotNullable()
                .WithColumn("creator").AsString(50).NotNullable()
                .WithColumn("updated_time").AsDateTimeOffset().NotNullable()
                .WithColumn("updater").AsString(50).NotNullable()
                .WithColumn("server_time").AsDateTimeOffset().WithDefault(SystemMethods.CurrentUTCDateTime);

            //create pk
            Create
                .PrimaryKey("tokentable_pk")
                .OnTable(TableName)
                .Columns("issuer", "audience");

            //update server_time use sql server instance time
            //https://x-team.com/blog/automatic-timestamps-with-postgresql/
            Execute.Sql("CREATE OR REPLACE FUNCTION trigger_set_timestamp() RETURNS TRIGGER AS $$ BEGIN NEW.server_time = NOW(); RETURN NEW; END; $$ LANGUAGE plpgsql; ");
            Execute.Sql($"CREATE TRIGGER set_timestamp BEFORE UPDATE ON {TableName} FOR EACH ROW EXECUTE PROCEDURE trigger_set_timestamp(); ");
        }
    }
}
