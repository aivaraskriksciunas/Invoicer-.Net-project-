using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoicer.Migrations
{
    /// <inheritdoc />
    public partial class addAmountFieldToRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "BillableRecords",
                type: "numeric(10,4)",
                nullable: true,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "BillableUnitId",
                table: "BillableRecords",
                type: "character varying(32)",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<double>(
                name: "PricePerUnit",
                table: "BillableRecords",
                type: "numeric(10,4)",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_BillableRecords_BillableUnitId",
                table: "BillableRecords",
                column: "BillableUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillableRecords_BillableUnits_BillableUnitId",
                table: "BillableRecords",
                column: "BillableUnitId",
                principalTable: "BillableUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillableRecords_BillableUnits_BillableUnitId",
                table: "BillableRecords");

            migrationBuilder.DropIndex(
                name: "IX_BillableRecords_BillableUnitId",
                table: "BillableRecords");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "BillableRecords");

            migrationBuilder.DropColumn(
                name: "BillableUnitId",
                table: "BillableRecords");

            migrationBuilder.DropColumn(
                name: "PricePerUnit",
                table: "BillableRecords");
        }
    }
}
