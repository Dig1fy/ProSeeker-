using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSeeker.Data.Migrations
{
    public partial class AdjustUsersGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_AspNetUsers_ApplicationUserId",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Groups_GroupId",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                newName: "UsersGroups");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_IsDeleted",
                table: "UsersGroups",
                newName: "IX_UsersGroups_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_GroupId",
                table: "UsersGroups",
                newName: "IX_UsersGroups_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_ApplicationUserId",
                table: "UsersGroups",
                newName: "IX_UsersGroups_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersGroups",
                table: "UsersGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroups_AspNetUsers_ApplicationUserId",
                table: "UsersGroups",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersGroups_Groups_GroupId",
                table: "UsersGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroups_AspNetUsers_ApplicationUserId",
                table: "UsersGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersGroups_Groups_GroupId",
                table: "UsersGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersGroups",
                table: "UsersGroups");

            migrationBuilder.RenameTable(
                name: "UsersGroups",
                newName: "UserGroups");

            migrationBuilder.RenameIndex(
                name: "IX_UsersGroups_IsDeleted",
                table: "UserGroups",
                newName: "IX_UserGroups_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_UsersGroups_GroupId",
                table: "UserGroups",
                newName: "IX_UserGroups_GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersGroups_ApplicationUserId",
                table: "UserGroups",
                newName: "IX_UserGroups_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_AspNetUsers_ApplicationUserId",
                table: "UserGroups",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Groups_GroupId",
                table: "UserGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
