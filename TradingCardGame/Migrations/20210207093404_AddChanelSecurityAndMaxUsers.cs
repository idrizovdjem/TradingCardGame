using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingCardGame.Migrations
{
    public partial class AddChanelSecurityAndMaxUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxUsers",
                table: "Channels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Security",
                table: "Channels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxUsers",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "Security",
                table: "Channels");
        }
    }
}
