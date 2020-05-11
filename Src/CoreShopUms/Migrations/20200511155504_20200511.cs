using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreShopUms.Migrations
{
    public partial class _20200511 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<string>("varchar(50)"),
                    Account = table.Column<string>("varchar(50)", nullable: true),
                    Password = table.Column<string>("varchar(300)", nullable: true),
                    RealName = table.Column<string>("varchar(50)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Users");
        }
    }
}