using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Dal.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNavigationPropertyFolderInTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_folders_folder_id",
                table: "tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_folders_folder_id",
                table: "tasks",
                column: "folder_id",
                principalTable: "folders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_folders_folder_id",
                table: "tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_folders_folder_id",
                table: "tasks",
                column: "folder_id",
                principalTable: "folders",
                principalColumn: "id");
        }
    }
}
