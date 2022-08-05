using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline.Inventory.Migrations
{
    public partial class normalticket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tblAirLine",
                keyColumn: "AirlineId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 8, 4, 10, 26, 34, 231, DateTimeKind.Local).AddTicks(8387));

            migrationBuilder.UpdateData(
                table: "tblAirLine",
                keyColumn: "AirlineId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2022, 8, 4, 10, 26, 34, 231, DateTimeKind.Local).AddTicks(8395));

            migrationBuilder.UpdateData(
                table: "tblFlight",
                keyColumn: "FlightID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 8, 4, 10, 26, 34, 231, DateTimeKind.Local).AddTicks(8434));

            migrationBuilder.UpdateData(
                table: "tblFlight",
                keyColumn: "FlightID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2022, 8, 4, 10, 26, 34, 231, DateTimeKind.Local).AddTicks(8444));

            migrationBuilder.UpdateData(
                table: "tblInventoy",
                keyColumn: "InventoryId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 8, 4, 10, 26, 34, 231, DateTimeKind.Local).AddTicks(7896), new DateTime(2022, 8, 14, 10, 26, 34, 231, DateTimeKind.Local).AddTicks(7885), new DateTime(2022, 8, 9, 10, 26, 34, 231, DateTimeKind.Local).AddTicks(7849) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tblAirLine",
                keyColumn: "AirlineId",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(347));

            migrationBuilder.UpdateData(
                table: "tblAirLine",
                keyColumn: "AirlineId",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(352));

            migrationBuilder.UpdateData(
                table: "tblFlight",
                keyColumn: "FlightID",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(373));

            migrationBuilder.UpdateData(
                table: "tblFlight",
                keyColumn: "FlightID",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(379));

            migrationBuilder.UpdateData(
                table: "tblInventoy",
                keyColumn: "InventoryId",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(166), new DateTime(2022, 8, 12, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(159), new DateTime(2022, 8, 7, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(138) });
        }
    }
}
