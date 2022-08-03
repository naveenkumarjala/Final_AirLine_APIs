using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Airline.Booking.Migrations
{
    public partial class newBookingMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblBookings",
                columns: table => new
                {
                    TicketID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfJourney = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoardingTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    passportNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TicketCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Statusstr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updatedby = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Seattype = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBookings", x => x.TicketID);
                });

            migrationBuilder.InsertData(
                table: "tblBookings",
                columns: new[] { "TicketID", "Age", "BoardingTime", "BookingID", "CreatedBy", "CreatedDate", "DateOfJourney", "Discount", "EmailID", "FlightNumber", "FromPlace", "SeatNumber", "Seattype", "Status", "Statusstr", "TicketCost", "ToPlace", "UpdatedDate", "Updatedby", "UserName", "passportNumber" },
                values: new object[] { "TICK585755", 25, "10.00 AM", "BCK125458", "naveen", new DateTime(2022, 8, 2, 18, 52, 45, 47, DateTimeKind.Local).AddTicks(1154), new DateTime(2022, 8, 12, 18, 52, 45, 47, DateTimeKind.Local).AddTicks(1087), 0m, "kakumani15@gmail.com", "V12345", "Hyderabd", 2, 1, 1, "TicketBooked", 0m, "BASR", new DateTime(2022, 8, 2, 18, 52, 45, 47, DateTimeKind.Local).AddTicks(1161), "naveen", "naveen", "V655585" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblBookings");
        }
    }
}
