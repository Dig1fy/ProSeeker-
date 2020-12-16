using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSeeker.Data.Migrations
{
    public partial class AdInquiriesOffersRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InquiryId",
                table: "Offer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offer_InquiryId",
                table: "Offer",
                column: "InquiryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Inquiries_InquiryId",
                table: "Offer",
                column: "InquiryId",
                principalTable: "Inquiries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Inquiries_InquiryId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Offer_InquiryId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "InquiryId",
                table: "Offer");
        }
    }
}
