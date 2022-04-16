using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlutaApp.Migrations
{
    public partial class addednew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TriviaQuestionId",
                table: "TriviaResults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TriviaResults_TriviaQuestionId",
                table: "TriviaResults",
                column: "TriviaQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TriviaResults_UserId",
                table: "TriviaResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TriviaAttempts_UserId",
                table: "TriviaAttempts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TriviaAttempts_Users_UserId",
                table: "TriviaAttempts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TriviaResults_TriviaQuestions_TriviaQuestionId",
                table: "TriviaResults",
                column: "TriviaQuestionId",
                principalTable: "TriviaQuestions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TriviaResults_Users_UserId",
                table: "TriviaResults",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TriviaAttempts_Users_UserId",
                table: "TriviaAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_TriviaResults_TriviaQuestions_TriviaQuestionId",
                table: "TriviaResults");

            migrationBuilder.DropForeignKey(
                name: "FK_TriviaResults_Users_UserId",
                table: "TriviaResults");

            migrationBuilder.DropIndex(
                name: "IX_TriviaResults_TriviaQuestionId",
                table: "TriviaResults");

            migrationBuilder.DropIndex(
                name: "IX_TriviaResults_UserId",
                table: "TriviaResults");

            migrationBuilder.DropIndex(
                name: "IX_TriviaAttempts_UserId",
                table: "TriviaAttempts");

            migrationBuilder.DropColumn(
                name: "TriviaQuestionId",
                table: "TriviaResults");
        }
    }
}
