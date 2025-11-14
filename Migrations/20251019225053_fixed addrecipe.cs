using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrzepisakApi.Migrations
{
    /// <inheritdoc />
    public partial class fixedaddrecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_recipes_author_id",
                table: "recipes",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "fk_recipes_users",
                table: "recipes",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_recipes_users",
                table: "recipes");

            migrationBuilder.DropIndex(
                name: "IX_recipes_author_id",
                table: "recipes");
        }
    }
}
