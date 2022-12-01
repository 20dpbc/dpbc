using FluentMigrator;

namespace dpbc.app.Migrations
{
    [Migration(1)]
    public class InitialTables : Migration
    {
        public override void Down()
        {
            Delete.Table("Point");
        }

        public override void Up()
        {
            Create.Table("Point")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt64().NotNullable().Unique()
                .WithColumn("message_id").AsInt64().NotNullable()
                .WithColumn("registerAt").AsDateTime().NotNullable(); ;
        }
    }
}
