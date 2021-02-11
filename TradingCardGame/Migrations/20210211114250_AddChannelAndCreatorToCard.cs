using Microsoft.EntityFrameworkCore.Migrations;

namespace TradingCardGame.Migrations
{
    public partial class AddChannelAndCreatorToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChannelId",
                table: "Cards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Cards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ChannelId",
                table: "Cards",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CreatorId",
                table: "Cards",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_AspNetUsers_CreatorId",
                table: "Cards",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Channels_ChannelId",
                table: "Cards",
                column: "ChannelId",
                principalTable: "Channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_AspNetUsers_CreatorId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Channels_ChannelId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_ChannelId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CreatorId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Cards");
        }
    }
}
