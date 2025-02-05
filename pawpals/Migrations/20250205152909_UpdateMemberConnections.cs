using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pawpals.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMemberConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.CreateTable(
                name: "MemberConnections",
                columns: table => new
                {
                    FollowersMemberId = table.Column<int>(type: "int", nullable: false),
                    FollowingMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberConnections", x => new { x.FollowersMemberId, x.FollowingMemberId });
                    table.ForeignKey(
                        name: "FK_MemberConnections_Members_FollowersMemberId",
                        column: x => x.FollowersMemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberConnections_Members_FollowingMemberId",
                        column: x => x.FollowingMemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MemberConnections_FollowingMemberId",
                table: "MemberConnections",
                column: "FollowingMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberConnections");

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    ConnectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FollowerId = table.Column<int>(type: "int", nullable: false),
                    FollowingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_Connections_Members_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_Members_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_FollowerId",
                table: "Connections",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_FollowingId",
                table: "Connections",
                column: "FollowingId");
        }
    }
}
