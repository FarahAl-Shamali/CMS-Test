using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class changedAdvOwnerIdToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisments_AspNetUsers_OwnerId",
                table: "Advertisments");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Advertisments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisments_AspNetUsers_OwnerId",
                table: "Advertisments",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisments_AspNetUsers_OwnerId",
                table: "Advertisments");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Advertisments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisments_AspNetUsers_OwnerId",
                table: "Advertisments",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
