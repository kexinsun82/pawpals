using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pawpals.Migrations
{
    /// <inheritdoc />
    public partial class TryFixError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Connections",
                type: "VARCHAR(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Connections",
                type: "VARCHAR(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_ApplicationUserId",
                table: "Connections",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_ApplicationUserId1",
                table: "Connections",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_AspNetUsers_ApplicationUserId",
                table: "Connections",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_AspNetUsers_ApplicationUserId1",
                table: "Connections",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_AspNetUsers_ApplicationUserId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_AspNetUsers_ApplicationUserId1",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_ApplicationUserId",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_ApplicationUserId1",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AspNetUsers");
        }
    }
}
