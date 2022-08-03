﻿// <auto-generated />
using System;
using Airline.Booking.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Airline.Booking.Migrations
{
    [DbContext(typeof(ApplicationBookDbcontext))]
    [Migration("20220802132245_newBookingMigration")]
    partial class newBookingMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Airline.Booking.Models.Bookings", b =>
                {
                    b.Property<string>("TicketID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("BoardingTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookingID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfJourney")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("EmailID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromPlace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("int");

                    b.Property<int>("Seattype")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Statusstr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TicketCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ToPlace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Updatedby")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("passportNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("TicketID");

                    b.ToTable("tblBookings");

                    b.HasData(
                        new
                        {
                            TicketID = "TICK585755",
                            Age = 25,
                            BoardingTime = "10.00 AM",
                            BookingID = "BCK125458",
                            CreatedBy = "naveen",
                            CreatedDate = new DateTime(2022, 8, 2, 18, 52, 45, 47, DateTimeKind.Local).AddTicks(1154),
                            DateOfJourney = new DateTime(2022, 8, 12, 18, 52, 45, 47, DateTimeKind.Local).AddTicks(1087),
                            Discount = 0m,
                            EmailID = "kakumani15@gmail.com",
                            FlightNumber = "V12345",
                            FromPlace = "Hyderabd",
                            SeatNumber = 2,
                            Seattype = 1,
                            Status = 1,
                            Statusstr = "TicketBooked",
                            TicketCost = 0m,
                            ToPlace = "BASR",
                            UpdatedDate = new DateTime(2022, 8, 2, 18, 52, 45, 47, DateTimeKind.Local).AddTicks(1161),
                            Updatedby = "naveen",
                            UserName = "naveen",
                            passportNumber = "V655585"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}