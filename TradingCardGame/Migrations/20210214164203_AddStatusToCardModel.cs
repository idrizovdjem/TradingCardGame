using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingCardGame.Migrations
{
    public partial class AddStatusToCardModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cards");
        }
    }
}
