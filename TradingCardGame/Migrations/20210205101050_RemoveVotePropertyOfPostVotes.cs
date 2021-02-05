using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingCardGame.Migrations
{
    public partial class RemoveVotePropertyOfPostVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "PostVotes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Value",
                table: "PostVotes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
