using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoicer.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullableUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillableUnits_AspNetUsers_UserId",
                table: "BillableUnits");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BillableUnits",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_BillableUnits_AspNetUsers_UserId",
                table: "BillableUnits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillableUnits_AspNetUsers_UserId",
                table: "BillableUnits");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BillableUnits",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BillableUnits_AspNetUsers_UserId",
                table: "BillableUnits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
