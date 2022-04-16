using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlutaApp.Migrations
{
    public partial class navigationproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ChatGroups_DepartmentId",
                table: "ChatGroups",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroups_InstitutionId",
                table: "ChatGroups",
                column: "InstitutionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatGroups_Departments_DepartmentId",
                table: "ChatGroups",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatGroups_Institutions_InstitutionId",
                table: "ChatGroups",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatGroups_Departments_DepartmentId",
                table: "ChatGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatGroups_Institutions_InstitutionId",
                table: "ChatGroups");

            migrationBuilder.DropIndex(
                name: "IX_ChatGroups_DepartmentId",
                table: "ChatGroups");

            migrationBuilder.DropIndex(
                name: "IX_ChatGroups_InstitutionId",
                table: "ChatGroups");
        }
    }
}
