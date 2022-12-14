using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline.Inventory.Migrations
{
    public partial class newInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAirLine",
                columns: table => new
                {
                    AirlineId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updatedby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAirLine", x => x.AirlineId);
                });

            migrationBuilder.CreateTable(
                name: "tblFlight",
                columns: table => new
                {
                    FlightID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AirLineId = table.Column<int>(type: "int", nullable: false),
                    FlightName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updatedby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFlight", x => x.FlightID);
                });

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

            migrationBuilder.InsertData(
                table: "tblAirLine",
                columns: new[] { "AirlineId", "ActiveStatus", "Address", "ContactNumber", "CreatedBy", "CreatedDate", "Name", "UpdatedDate", "Updatedby" },
                values: new object[,]
                {
                    { 1, "Y", "Hyderabad", "9948757854", "Admin", new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(347), "Cargo", null, null },
                    { 2, "Y", "Banglore", "9948077376", "Admin", new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(352), "Indigo", null, null }
                });

            migrationBuilder.InsertData(
                table: "tblFlight",
                columns: new[] { "FlightID", "AirLineId", "CreatedBy", "CreatedDate", "FlightName", "FlightNumber", "UpdatedDate", "Updatedby" },
                values: new object[,]
                {
                    { 1, 1, "Admin", new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(373), "Enfield", "V12345", null, null },
                    { 2, 2, "Admin", new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(379), "FZ-MR", "VF12345", null, null }
                });

            migrationBuilder.InsertData(
                table: "tblInventoy",
                columns: new[] { "InventoryId", "AirLineId", "BClassCount", "BclassAvailCount", "CreatedBy", "CreatedDate", "Discount", "EndDate", "EndTime", "FlightNumber", "FromPlace", "Instrument", "IsActive", "Meal", "NBClassCount", "NBclassAvailableCount", "Rows", "ScheduledDays", "StartDate", "TicketCoopun", "TicketCost", "ToPlace", "UpdatedDate", "Updatedby", "startTime" },
                values: new object[] { 1, 1, 50, 50, "Admin", new DateTime(2022, 8, 2, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(166), 10m, new DateTime(2022, 8, 12, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(159), "12.00 PM", "V12345", "BASAR", "Insur", " ", 3, 100, 100, 5, 8, new DateTime(2022, 8, 7, 18, 48, 3, 441, DateTimeKind.Local).AddTicks(138), "CP10", 2000m, "HYDERABAD", null, null, "10.00 AM" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAirLine");

            migrationBuilder.DropTable(
                name: "tblFlight");

            migrationBuilder.DropTable(
                name: "tblInventoy");
        }
    }
}
