using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Educational.Migrations
{
    /// <inheritdoc />
    public partial class 教室表 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DictItem_DictType_DictTypeId",
                table: "DictItem");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_DictType_TempId",
                table: "DictType");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "AppCourse");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "DictType");

            migrationBuilder.RenameTable(
                name: "DictType",
                newName: "dict_type");

            migrationBuilder.RenameTable(
                name: "DictItem",
                newName: "dict_item");

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

            migrationBuilder.AddColumn<Guid>(
                name: "GratorId",
                table: "AppCourse",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "dict_type",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "dict_type",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "dict_type",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "dict_type",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "dict_type",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "dict_type",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "dict_type",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "dict_type",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "dict_type",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "dict_type",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "dict_type",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "dict_type",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "DictTypeId",
                table: "dict_item",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "dict_item",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "dict_item",
                type: "varchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "dict_item",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "dict_item",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "dict_item",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "dict_item",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "dict_item",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "dict_item",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "dict_item",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "dict_item",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "dict_item",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "dict_item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_dict_type",
                table: "dict_type",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dict_item",
                table: "dict_item",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ClassRoom",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClassRoomName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrganizationModelId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClassRoomAddress = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClassRoomArea = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ClassRoomDescription = table.Column<string>(type: "longtext", nullable: false)
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
                    table.PrimaryKey("PK_ClassRoom", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_dict_item_DictTypeId",
                table: "dict_item",
                column: "DictTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_dict_item_dict_type_DictTypeId",
                table: "dict_item",
                column: "DictTypeId",
                principalTable: "dict_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dict_item_dict_type_DictTypeId",
                table: "dict_item");

            migrationBuilder.DropTable(
                name: "ClassRoom");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dict_type",
                table: "dict_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dict_item",
                table: "dict_item");

            migrationBuilder.DropIndex(
                name: "IX_dict_item_DictTypeId",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "GratorId",
                table: "AppCourse");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "dict_type");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "dict_item");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "dict_item");

            migrationBuilder.RenameTable(
                name: "dict_type",
                newName: "DictType");

            migrationBuilder.RenameTable(
                name: "dict_item",
                newName: "DictItem");

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

            migrationBuilder.AddColumn<Guid>(
                name: "GradeId",
                table: "AppCourse",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "DictType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DictTypeId",
                table: "DictItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_DictType_TempId",
                table: "DictType",
                column: "TempId");

            migrationBuilder.AddForeignKey(
                name: "FK_DictItem_DictType_DictTypeId",
                table: "DictItem",
                column: "DictTypeId",
                principalTable: "DictType",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
