using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Dal.Migrations
{
    /// <inheritdoc />
    public partial class DescriptionCanBeNullInUserTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "tasks",
                type: "varchar",
                maxLength: 100000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 100000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "tasks",
                type: "varchar",
                maxLength: 100000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 100000,
                oldNullable: true);
        }
    }
}
