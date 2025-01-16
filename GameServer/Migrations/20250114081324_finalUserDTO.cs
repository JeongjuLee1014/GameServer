using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameServer.Migrations
{
    /// <inheritdoc />
    public partial class finalUserDTO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sessionId",
                table: "Users",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "numStars",
                table: "Users",
                newName: "NumStars");

            migrationBuilder.RenameColumn(
                name: "numEnergies",
                table: "Users",
                newName: "NumEnergies");

            migrationBuilder.RenameColumn(
                name: "numCoins",
                table: "Users",
                newName: "NumCoins");

            migrationBuilder.RenameColumn(
                name: "nickName",
                table: "Users",
                newName: "NickName");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Users",
                newName: "sessionId");

            migrationBuilder.RenameColumn(
                name: "NumStars",
                table: "Users",
                newName: "numStars");

            migrationBuilder.RenameColumn(
                name: "NumEnergies",
                table: "Users",
                newName: "numEnergies");

            migrationBuilder.RenameColumn(
                name: "NumCoins",
                table: "Users",
                newName: "numCoins");

            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "Users",
                newName: "nickName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");
        }
    }
}
