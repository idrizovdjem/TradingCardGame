using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingCardGame.Migrations
{
    public partial class AddCardEffectField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Effect",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Effect",
                table: "Cards");
        }
    }
}
