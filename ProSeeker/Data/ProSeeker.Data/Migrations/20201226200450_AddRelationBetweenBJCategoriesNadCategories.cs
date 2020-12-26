using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSeeker.Data.Migrations
{
    public partial class AddRelationBetweenBJCategoriesNadCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobCategoryId",
                table: "JobCategories",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobCategoryId",
                table: "JobCategories");
        }
    }
}
