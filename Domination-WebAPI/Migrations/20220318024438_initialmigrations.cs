using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domination_WebAPI.Migrations
{
    public partial class initialmigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    xCoord = table.Column<int>(type: "int", nullable: false),
                    yCoord = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameZones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NodeImprovement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImgURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    ResourceType = table.Column<int>(type: "int", nullable: false),
                    StaticBonus = table.Column<int>(type: "int", nullable: false),
                    Modifier = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeImprovement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchTech",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResearchCost = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchTech", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Food = table.Column<int>(type: "int", nullable: false),
                    Culture = table.Column<int>(type: "int", nullable: false),
                    Population = table.Column<int>(type: "int", nullable: false),
                    Research = table.Column<int>(type: "int", nullable: false),
                    IndustrialGoods = table.Column<int>(type: "int", nullable: false),
                    Money = table.Column<int>(type: "int", nullable: false),
                    MilitaryMight = table.Column<int>(type: "int", nullable: false),
                    AirForce = table.Column<int>(type: "int", nullable: false),
                    Army = table.Column<int>(type: "int", nullable: false),
                    Navy = table.Column<int>(type: "int", nullable: false),
                    AcquisitionPoints = table.Column<int>(type: "int", nullable: false),
                    NationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adjective = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    xCoord = table.Column<int>(type: "int", nullable: false),
                    yCoord = table.Column<int>(type: "int", nullable: false),
                    LastProducedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentResourceType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameZodeId = table.Column<int>(type: "int", nullable: false),
                    GameZoneId = table.Column<int>(type: "int", nullable: false),
                    IsDevelopable = table.Column<bool>(type: "bit", nullable: false),
                    NodeQuality = table.Column<int>(type: "int", nullable: false),
                    NodeImprovementId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameNodes_GameZones_GameZoneId",
                        column: x => x.GameZoneId,
                        principalTable: "GameZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameNodes_NodeImprovement_NodeImprovementId",
                        column: x => x.NodeImprovementId,
                        principalTable: "NodeImprovement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bonuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modifier = table.Column<double>(type: "float", nullable: false),
                    Resource = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResearchTechId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bonuses_ResearchTech_ResearchTechId",
                        column: x => x.ResearchTechId,
                        principalTable: "ResearchTech",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerResearchTechs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ResearchTechId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerResearchTechs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerResearchTechs_ResearchTech_ResearchTechId",
                        column: x => x.ResearchTechId,
                        principalTable: "ResearchTech",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerResearchTechs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerBonus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BonusId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBonus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerBonus_Bonuses_BonusId",
                        column: x => x.BonusId,
                        principalTable: "Bonuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerBonus_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bonuses_ResearchTechId",
                table: "Bonuses",
                column: "ResearchTechId");

            migrationBuilder.CreateIndex(
                name: "IX_GameNodes_GameZoneId",
                table: "GameNodes",
                column: "GameZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_GameNodes_NodeImprovementId",
                table: "GameNodes",
                column: "NodeImprovementId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBonus_BonusId",
                table: "PlayerBonus",
                column: "BonusId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBonus_UserId",
                table: "PlayerBonus",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerResearchTechs_ResearchTechId",
                table: "PlayerResearchTechs",
                column: "ResearchTechId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerResearchTechs_UserId",
                table: "PlayerResearchTechs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameNodes");

            migrationBuilder.DropTable(
                name: "PlayerBonus");

            migrationBuilder.DropTable(
                name: "PlayerResearchTechs");

            migrationBuilder.DropTable(
                name: "GameZones");

            migrationBuilder.DropTable(
                name: "NodeImprovement");

            migrationBuilder.DropTable(
                name: "Bonuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ResearchTech");
        }
    }
}
