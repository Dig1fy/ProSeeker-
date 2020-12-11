using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSeeker.Data.Migrations
{
    public partial class AddAdToOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AdId",
                table: "Offer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_AdId",
                table: "Offer",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Ads_AdId",
                table: "Offer",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Ads_AdId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_AdId",
                table: "Offer");

            migrationBuilder.AlterColumn<string>(
                name: "AdId",
                table: "Offer",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
