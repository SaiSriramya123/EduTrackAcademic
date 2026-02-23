using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrackAcademics.Migrations
{
    /// <inheritdoc />
    public partial class initialcrea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coordinator",
                columns: table => new
                {
                    CoordinatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CoordinatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorPhone = table.Column<long>(type: "bigint", nullable: false),
                    CoordinatorQualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorExperience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordinatorPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFirstLogin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinator", x => x.CoordinatorId);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Program_ID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditPoints = table.Column<int>(type: "int", nullable: false),
                    CourseStatus = table.Column<bool>(type: "bit", nullable: false),
                    CourseDuration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Instructor",
                columns: table => new
                {
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstructorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstructorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstructorPhone = table.Column<long>(type: "bigint", nullable: false),
                    InstructorQualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstructorSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstructorExperience = table.Column<int>(type: "int", nullable: false),
                    InstructorJoinDate = table.Column<DateOnly>(type: "date", nullable: false),
                    InstructorGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstructorPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructor", x => x.InstructorId);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentPhone = table.Column<long>(type: "bigint", nullable: false),
                    StudentQualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentProgram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentAcademicYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coordinator");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Instructor");

            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}
