using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class redundantColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GlassStormIoData_Locations_Locations",
                table: "GlassStormIoData");

            migrationBuilder.DropIndex(
                name: "IX_GlassStormIoData_Locations",
                table: "GlassStormIoData");

            migrationBuilder.DropColumn(
                name: "Locations",
                table: "GlassStormIoData");

            migrationBuilder.CreateIndex(
                name: "IX_GlassStormIoData_LocationId",
                table: "GlassStormIoData",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_GlassStormIoData_Locations_LocationId",
                table: "GlassStormIoData",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GlassStormIoData_Locations_LocationId",
                table: "GlassStormIoData");

            migrationBuilder.DropIndex(
                name: "IX_GlassStormIoData_LocationId",
                table: "GlassStormIoData");

            migrationBuilder.AddColumn<int>(
                name: "Locations",
                table: "GlassStormIoData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GlassStormIoData_Locations",
                table: "GlassStormIoData",
                column: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_GlassStormIoData_Locations_Locations",
                table: "GlassStormIoData",
                column: "Locations",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
