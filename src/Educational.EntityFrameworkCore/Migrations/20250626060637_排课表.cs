using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Educational.Migrations
{
    /// <inheritdoc />
    public partial class 排课表 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "StaffTypeId",
                table: "AppStaffInfo",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                comment: "人员类型",
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(50)",
                oldMaxLength: 50,
                oldComment: "人员类型")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "AppStaffInfo",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                comment: "权限角色",
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(50)",
                oldMaxLength: 50,
                oldComment: "权限角色")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "PositionId",
                table: "AppStaffInfo",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                comment: "职位",
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(50)",
                oldMaxLength: 50,
                oldComment: "职位")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "AppSpecialSubjectModel",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AchievementDisplay",
                table: "AppSpecialSubjectModel",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "AppCategoryModel",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppClassSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CampusId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClassId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CourseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MainTeacher = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssistantTeacher = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ConsumptionBase = table.Column<int>(type: "int", nullable: false),
                    MaxAttendees = table.Column<int>(type: "int", nullable: true),
                    MaxSchedules = table.Column<int>(type: "int", nullable: true),
                    SkipHolidays = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClassSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppClassSchedule_AppClassInfo_ClassId",
                        column: x => x.ClassId,
                        principalTable: "AppClassInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppClassSchedule_AppCourse_CourseId",
                        column: x => x.CourseId,
                        principalTable: "AppCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppClassSchedule_AppOrganizationModel_CampusId",
                        column: x => x.CampusId,
                        principalTable: "AppOrganizationModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AppScheduleTime",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ClassroomName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClassScheduleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppScheduleTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppScheduleTime_AppClassSchedule_ClassScheduleId",
                        column: x => x.ClassScheduleId,
                        principalTable: "AppClassSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AppClassSchedule_CampusId",
                table: "AppClassSchedule",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_AppClassSchedule_ClassId",
                table: "AppClassSchedule",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AppClassSchedule_CourseId",
                table: "AppClassSchedule",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AppScheduleTime_ClassScheduleId",
                table: "AppScheduleTime",
                column: "ClassScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppScheduleTime");

            migrationBuilder.DropTable(
                name: "AppClassSchedule");

            migrationBuilder.AlterColumn<string>(
                name: "StaffTypeId",
                table: "AppStaffInfo",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                comment: "人员类型",
                oldClrType: typeof(Guid),
                oldType: "char(50)",
                oldMaxLength: 50,
                oldComment: "人员类型")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AppStaffInfo",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                comment: "权限角色",
                oldClrType: typeof(Guid),
                oldType: "char(50)",
                oldMaxLength: 50,
                oldComment: "权限角色")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "PositionId",
                table: "AppStaffInfo",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                comment: "职位",
                oldClrType: typeof(Guid),
                oldType: "char(50)",
                oldMaxLength: 50,
                oldComment: "职位")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "AppSpecialSubjectModel",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AchievementDisplay",
                table: "AppSpecialSubjectModel",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "AppCategoryModel",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
