using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameServer.Migrations
{
    /// <inheritdoc />
    public partial class UserDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Users",
                newName: "sessionId");

            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "Users",
                newName: "nickName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sessionId",
                table: "Users",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "nickName",
                table: "Users",
                newName: "NickName");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");
        }
    }
}
