using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline.Booking.Migrations
{
    public partial class newBookingMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblInventoy",
                columns: table => new
                {
                    InventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AirLineId = table.Column<int>(type: "int", nullable: false),
                    FromPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    startTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledDays = table.Column<int>(type: "int", nullable: false),
                    Instrument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BClassCount = table.Column<int>(type: "int", nullable: false),
                    BclassAvailCount = table.Column<int>(type: "int", nullable: false),
                    NBClassCount = table.Column<int>(type: "int", nullable: false),
                    NBclassAvailableCount = table.Column<int>(type: "int", nullable: false),
                    TicketCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TicketCoopun = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rows = table.Column<int>(type: "int", nullable: false),
                    Meal = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updatedby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInventoy", x => x.InventoryId);
                });

            migrationBuilder.UpdateData(
                table: "tblBookings",
                keyColumn: "TicketID",
                keyValue: "TICK585755",
                columns: new[] { "CreatedDate", "DateOfJourney", "UpdatedDate" },
                values: new object[] { new DateTime(2022, 8, 2, 18, 53, 52, 367, DateTimeKind.Local).AddTicks(5559), new DateTime(2022, 8, 12, 18, 53, 52, 367, DateTimeKind.Local).AddTicks(5489), new DateTime(2022, 8, 2, 18, 53, 52, 367, DateTimeKind.Local).AddTicks(5561) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblInventoy");

            migrationBuilder.UpdateData(
                table: "tblBookings",
                keyColumn: "TicketID",
                keyValue: "TICK585755",
                columns: new[] { "CreatedDate", "DateOfJourney", "UpdatedDate" },
                values: new object[] { new DateTime(2022, 8, 2, 18, 52, 45, 47, DateTimeKind.Local).AddTicks(1154), new DateTime(2022, 8, 12, 18, 52, 45, 47, DateTimeKind.Local).AddTicks(1087), new DateTime(2022, 8, 2, 18, 52, 45, 47, DateTimeKind.Local).AddTicks(1161) });
        }
    }
}
