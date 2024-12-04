using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class removeUniqueConstInTrainingLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainingLog_ApplicationUserId",
                table: "TrainingLog");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLog_ApplicationUserId",
                table: "TrainingLog",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainingLog_ApplicationUserId",
                table: "TrainingLog");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLog_ApplicationUserId",
                table: "TrainingLog",
                column: "ApplicationUserId",
                unique: true);
        }
    }
}
