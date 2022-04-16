using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlutaApp.Migrations
{
    public partial class postnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Posts_PostId",
                table: "Promotions");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Promotions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Posts_PostId",
                table: "Promotions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Posts_PostId",
                table: "Promotions");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Posts_PostId",
                table: "Promotions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
