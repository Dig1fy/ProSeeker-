using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSeeker.Data.Migrations
{
    public partial class makeOffersSoftDeletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Offer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Offer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Offer_IsDeleted",
                table: "Offer",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Offer_IsDeleted",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Offer");
        }
    }
}
