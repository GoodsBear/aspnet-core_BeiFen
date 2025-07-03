using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Educational.Migrations
{
    /// <inheritdoc />
    public partial class 修改排课表 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppConflictModel_AppClassSchedule_ClassScheduleId",
                table: "AppConflictModel");

            migrationBuilder.DropForeignKey(
                name: "FK_AppScheduleTime_AppClassSchedule_ClassScheduleId",
                table: "AppScheduleTime");

            migrationBuilder.DropIndex(
                name: "IX_AppScheduleTime_ClassScheduleId",
                table: "AppScheduleTime");

            migrationBuilder.DropIndex(
                name: "IX_AppConflictModel_ClassScheduleId",
                table: "AppConflictModel");

            migrationBuilder.RenameColumn(
                name: "CampusId",
                table: "AppClassSchedule",
                newName: "OrganizationId");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "AppScheduleTime",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "AppScheduleTime",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassScheduleId",
                table: "AppConflictModel",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "AppClassSchedule",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "ConflictId",
                table: "AppClassSchedule",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "AppClassSchedule",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationName",
                table: "AppClassSchedule",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleTimeId",
                table: "AppClassSchedule",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "AppClassSchedule");

            migrationBuilder.DropColumn(
                name: "ConflictId",
                table: "AppClassSchedule");

            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "AppClassSchedule");

            migrationBuilder.DropColumn(
                name: "OrganizationName",
                table: "AppClassSchedule");

            migrationBuilder.DropColumn(
                name: "ScheduleTimeId",
                table: "AppClassSchedule");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "AppClassSchedule",
                newName: "CampusId");

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

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "AppScheduleTime",
                type: "time(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "AppScheduleTime",
                type: "time(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClassScheduleId",
                table: "AppConflictModel",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_AppScheduleTime_ClassScheduleId",
                table: "AppScheduleTime",
                column: "ClassScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppConflictModel_ClassScheduleId",
                table: "AppConflictModel",
                column: "ClassScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppConflictModel_AppClassSchedule_ClassScheduleId",
                table: "AppConflictModel",
                column: "ClassScheduleId",
                principalTable: "AppClassSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppScheduleTime_AppClassSchedule_ClassScheduleId",
                table: "AppScheduleTime",
                column: "ClassScheduleId",
                principalTable: "AppClassSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
