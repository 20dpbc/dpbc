using FluentMigrator;

namespace dpbc.repository.Migrations
{
    [Migration(1)]
    public class InitialTables : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Create.Table("Point")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt64().NotNullable()
                .WithColumn("message_id").AsInt64().NotNullable()
                .WithColumn("started").AsDateTime().NotNullable()
                .WithColumn("stoped").AsDateTime().Nullable();

            Create.Table("User")
                .WithColumn("id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("name").AsString().NotNullable().Unique()
                .WithColumn("uuid").AsString().NotNullable()
                .WithColumn("mention").AsString().NotNullable().Unique()
                .WithColumn("ra").AsString().NotNullable().Unique()
                .WithColumn("url").AsString()
                .WithColumn("created").AsDateTime().NotNullable();

            Create
                .Index()
                .OnTable("Point")
                .OnColumn("user_id");

            Create
                .Index()
                .OnTable("Point")
                .OnColumn("started");

            Create
                .Index()
                .OnTable("User")
                .OnColumn("uuid");
        }
    }
}
