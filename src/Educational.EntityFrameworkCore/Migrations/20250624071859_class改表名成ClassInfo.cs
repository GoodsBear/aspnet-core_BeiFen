using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Educational.Migrations
{
    /// <inheritdoc />
    public partial class class改表名成ClassInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppClass",
                table: "AppClass");

            migrationBuilder.RenameTable(
                name: "AppClass",
                newName: "AppClassInfo");

            migrationBuilder.AlterColumn<Guid>(
                name: "GradeName",
                table: "AppGrade",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(50)",
                oldMaxLength: 50)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppClassInfo",
                table: "AppClassInfo",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppClassInfo",
                table: "AppClassInfo");

            migrationBuilder.RenameTable(
                name: "AppClassInfo",
                newName: "AppClass");

            migrationBuilder.AlterColumn<string>(
                name: "GradeName",
                table: "AppGrade",
                type: "char(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppClass",
                table: "AppClass",
                column: "Id");
        }
    }
}
