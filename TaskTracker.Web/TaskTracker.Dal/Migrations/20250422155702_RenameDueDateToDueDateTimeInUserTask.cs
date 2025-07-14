using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Dal.Migrations
{
    /// <inheritdoc />
    public partial class RenameDueDateToDueDateTimeInUserTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "due_date",
                table: "tasks",
                newName: "due_date_time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "due_date_time",
                table: "tasks",
                newName: "due_date");
        }
    }
}
