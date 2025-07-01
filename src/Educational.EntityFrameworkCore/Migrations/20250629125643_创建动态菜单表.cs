using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Educational.Migrations
{
    /// <inheritdoc />
    public partial class 创建动态菜单表 : Migration
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

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MenuName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuPath = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuComponent = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuIcon = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuSort = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuType = table.Column<int>(type: "int", nullable: false),
                    MenuPermission = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuStatus = table.Column<int>(type: "int", nullable: false),
                    MenuHidden = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MenuRemark = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
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
                    table.PrimaryKey("PK_Menu", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menu");

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
        }
    }
}
