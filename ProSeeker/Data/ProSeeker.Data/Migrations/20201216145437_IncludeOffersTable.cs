using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSeeker.Data.Migrations
{
    public partial class IncludeOffersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Ads_AdId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_AspNetUsers_ApplicationUserId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Inquiries_InquiryId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Specialist_Details_SpecialistDetailsId",
                table: "Offer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "Offers");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_SpecialistDetailsId",
                table: "Offers",
                newName: "IX_Offers_SpecialistDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_IsDeleted",
                table: "Offers",
                newName: "IX_Offers_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_InquiryId",
                table: "Offers",
                newName: "IX_Offers_InquiryId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_ApplicationUserId",
                table: "Offers",
                newName: "IX_Offers_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_AdId",
                table: "Offers",
                newName: "IX_Offers_AdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Ads_AdId",
                table: "Offers",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_ApplicationUserId",
                table: "Offers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Inquiries_InquiryId",
                table: "Offers",
                column: "InquiryId",
                principalTable: "Inquiries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Specialist_Details_SpecialistDetailsId",
                table: "Offers",
                column: "SpecialistDetailsId",
                principalTable: "Specialist_Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Ads_AdId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_ApplicationUserId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Inquiries_InquiryId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Specialist_Details_SpecialistDetailsId",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Offer");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_SpecialistDetailsId",
                table: "Offer",
                newName: "IX_Offer_SpecialistDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_IsDeleted",
                table: "Offer",
                newName: "IX_Offer_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_InquiryId",
                table: "Offer",
                newName: "IX_Offer_InquiryId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_ApplicationUserId",
                table: "Offer",
                newName: "IX_Offer_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_AdId",
                table: "Offer",
                newName: "IX_Offer_AdId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Ads_AdId",
                table: "Offer",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_AspNetUsers_ApplicationUserId",
                table: "Offer",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Inquiries_InquiryId",
                table: "Offer",
                column: "InquiryId",
                principalTable: "Inquiries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Specialist_Details_SpecialistDetailsId",
                table: "Offer",
                column: "SpecialistDetailsId",
                principalTable: "Specialist_Details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
