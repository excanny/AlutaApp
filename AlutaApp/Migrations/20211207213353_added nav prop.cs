using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlutaApp.Migrations
{
    public partial class addednavprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CGPAs_UserId",
                table: "CGPAs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CGPAs_Users_UserId",
                table: "CGPAs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CGPAs_Users_UserId",
                table: "CGPAs");

            migrationBuilder.DropIndex(
                name: "IX_CGPAs_UserId",
                table: "CGPAs");
        }
    }
}
