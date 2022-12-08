using FluentMigrator;

namespace dpbc.app.Migrations
{
    [Migration(2)]
    public class PointColumns : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Create
                .Index()
                .OnTable("Point")
                .OnColumn("user_id");

            Create
                .Index()
                .OnTable("Point")
                .OnColumn("started");
        }
    }
}
