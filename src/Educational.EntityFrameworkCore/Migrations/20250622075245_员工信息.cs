using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Educational.Migrations
{
    /// <inheritdoc />
    public partial class 员工信息 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppStaffInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StaffName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "姓名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StaffAccount = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "登录账号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StaffPassword = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "登录密码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StaffPhone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "电话")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Organization = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "所属机构")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StaffGender = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, comment: "性别")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Position = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "职位")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "权限角色")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StaffType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "人员类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntryDate = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "入职日期"),
                    Status = table.Column<int>(type: "int", maxLength: 20, nullable: false, comment: "状态"),
                    Education = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "学历")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Birthday = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "生日"),
                    GraduationSchool = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "毕业学校")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Introduction = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "简介")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhotoUrl = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "照片")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppStaffInfo", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppStaffInfo");
        }
    }
}
