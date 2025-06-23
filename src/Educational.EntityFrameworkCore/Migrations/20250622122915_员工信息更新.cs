using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Educational.Migrations
{
    /// <inheritdoc />
    public partial class 员工信息更新 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StaffType",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "人员类型",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "人员类型")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffPhone",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "电话",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "电话")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffPassword",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "登录密码",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "登录密码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffName",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "姓名",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "姓名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffGender",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "性别",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldComment: "性别")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffAccount",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "登录账号",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "登录账号")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "权限角色",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldComment: "权限角色")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "职位",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "职位")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "照片",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldComment: "照片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Organization",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "所属机构",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldComment: "所属机构")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Introduction",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "简介",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldComment: "简介")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GraduationSchool",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "毕业学校",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldComment: "毕业学校")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Education",
                table: "AppStaffInfo",
                type: "longtext",
                nullable: false,
                comment: "学历",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "学历")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StaffType",
                table: "AppStaffInfo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "人员类型",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "人员类型")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffPhone",
                table: "AppStaffInfo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "电话",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "电话")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffPassword",
                table: "AppStaffInfo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "登录密码",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "登录密码")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffName",
                table: "AppStaffInfo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "姓名",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "姓名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffGender",
                table: "AppStaffInfo",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                comment: "性别",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "性别")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "StaffAccount",
                table: "AppStaffInfo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "登录账号",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "登录账号")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "AppStaffInfo",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "权限角色",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "权限角色")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "AppStaffInfo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "职位",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "职位")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoUrl",
                table: "AppStaffInfo",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "照片",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "照片")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Organization",
                table: "AppStaffInfo",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "所属机构",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "所属机构")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Introduction",
                table: "AppStaffInfo",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                comment: "简介",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "简介")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GraduationSchool",
                table: "AppStaffInfo",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "毕业学校",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "毕业学校")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Education",
                table: "AppStaffInfo",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "学历",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "学历")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
