using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrackAcademics.Migrations
{
    /// <inheritdoc />
    public partial class rules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicRules",
                columns: table => new
                {
                    RuleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RuleValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicRules", x => x.RuleId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicRules");
        }
    }
}
