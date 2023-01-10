using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModMenu.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserModMenus",
                columns: table => new
                {
                    UserModMenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ModMenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModMenus", x => x.UserModMenuId);
                    table.ForeignKey(
                        name: "FK_UserModMenus_ModMenus_ModMenuId",
                        column: x => x.ModMenuId,
                        principalTable: "ModMenus",
                        principalColumn: "ModMenuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserModMenus_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserModMenus_ModMenuId",
                table: "UserModMenus",
                column: "ModMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModMenus_UserId",
                table: "UserModMenus",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserModMenus");
        }
    }
}
