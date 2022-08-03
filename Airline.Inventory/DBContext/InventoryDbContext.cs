﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Airline.Inventory.Models;

namespace Airline.Inventory.DBContext
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {

        }
        public DbSet<Inventorys> tblInventoy { get; set; }
        public DbSet<AirLine> tblAirLine { get; set; }
        public DbSet<Flight> tblFlight { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Inventorys>().HasData(
            new Inventorys
            {
                InventoryId = 1,
                FlightNumber = "V12345",
                AirLineId = 1,
                FromPlace = "BASAR",
                ToPlace = "HYDERABAD",
                StartDate = DateTime.Now.AddDays(5),
                EndDate = DateTime.Now.AddDays(10),
                startTime = "10.00 AM",
                EndTime = "12.00 PM",
                ScheduledDays = flightAvailable.Daily,
                Instrument = "Insur",
                BClassCount = 50,
                BclassAvailCount = 50,
                NBClassCount = 100,
                NBclassAvailableCount = 100,
                TicketCost = 2000,
                Discount = 10,
                TicketCoopun = "CP10",
                Rows = 5,
                Meal = Meals.NonVeg,
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                //Updatedby = "Admin",
                //UpdatedDate = DateTime.Now
            }
            ) ; 
            model.Entity<AirLine>().HasData(
               new AirLine
               {
                   AirlineId = 1,
                   Name = "Cargo",
                   Address = "Hyderabad",
                   ContactNumber = "9948757854",
                   ActiveStatus = "Y",
                   CreatedBy = "Admin",
                   CreatedDate = DateTime.Now
               },
                new AirLine
                {
                    AirlineId = 2,
                    Name = "Indigo",
                    Address = "Banglore",
                    ContactNumber = "9948077376",
                     ActiveStatus = "Y",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now
                }
               );
            model.Entity<Flight>().HasData(
               new Flight
               {
                   FlightID = 1,
                   FlightNumber = "V12345",
                   AirLineId = 1,
                   FlightName = "Enfield",
                   CreatedBy = "Admin",
                   CreatedDate = DateTime.Now

               },
                new Flight
                {
                    FlightID = 2,
                    FlightNumber = "VF12345",
                    AirLineId = 2,
                    FlightName = "FZ-MR",
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now
                }
               ); 
          

        }
    }
}
