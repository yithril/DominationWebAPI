using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domination_WebAPI.Migrations
{
    public partial class nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameNodes_GameZones_GameZoneId",
                table: "GameNodes");

            migrationBuilder.DropForeignKey(
                name: "FK_GameNodes_NodeImprovement_NodeImprovementId",
                table: "GameNodes");

            migrationBuilder.DropIndex(
                name: "IX_GameNodes_GameZoneId",
                table: "GameNodes");

            migrationBuilder.DropIndex(
                name: "IX_GameNodes_NodeImprovementId",
                table: "GameNodes");

            migrationBuilder.DropColumn(
                name: "GameZoneId",
                table: "GameNodes");

            migrationBuilder.AlterColumn<int>(
                name: "NodeImprovementId",
                table: "GameNodes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_GameNodes_GameZodeId",
                table: "GameNodes",
                column: "GameZodeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameNodes_NodeImprovementId",
                table: "GameNodes",
                column: "NodeImprovementId",
                unique: true,
                filter: "[NodeImprovementId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_GameNodes_GameZones_GameZodeId",
                table: "GameNodes",
                column: "GameZodeId",
                principalTable: "GameZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameNodes_NodeImprovement_NodeImprovementId",
                table: "GameNodes",
                column: "NodeImprovementId",
                principalTable: "NodeImprovement",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameNodes_GameZones_GameZodeId",
                table: "GameNodes");

            migrationBuilder.DropForeignKey(
                name: "FK_GameNodes_NodeImprovement_NodeImprovementId",
                table: "GameNodes");

            migrationBuilder.DropIndex(
                name: "IX_GameNodes_GameZodeId",
                table: "GameNodes");

            migrationBuilder.DropIndex(
                name: "IX_GameNodes_NodeImprovementId",
                table: "GameNodes");

            migrationBuilder.AlterColumn<int>(
                name: "NodeImprovementId",
                table: "GameNodes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameZoneId",
                table: "GameNodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GameNodes_GameZoneId",
                table: "GameNodes",
                column: "GameZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_GameNodes_NodeImprovementId",
                table: "GameNodes",
                column: "NodeImprovementId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameNodes_GameZones_GameZoneId",
                table: "GameNodes",
                column: "GameZoneId",
                principalTable: "GameZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameNodes_NodeImprovement_NodeImprovementId",
                table: "GameNodes",
                column: "NodeImprovementId",
                principalTable: "NodeImprovement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
