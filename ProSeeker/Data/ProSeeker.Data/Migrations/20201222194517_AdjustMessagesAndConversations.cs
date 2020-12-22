using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSeeker.Data.Migrations
{
    public partial class AdjustMessagesAndConversations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "ChatMessages");

            migrationBuilder.AddColumn<bool>(
                name: "IsSeenBeReceiver",
                table: "Conversations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeenBySender",
                table: "Conversations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeenBeReceiver",
                table: "ChatMessages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeenBySender",
                table: "ChatMessages",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeenBeReceiver",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "IsSeenBySender",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "IsSeenBeReceiver",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "IsSeenBySender",
                table: "ChatMessages");

            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "Conversations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "ChatMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
