﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Airline.Inventory.ViewModel
{
    public class InventoryViewModel
    {
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        public int AirLineId { get; set; }
        [Required]
        public string FromPlace { get; set; }
        [Required]
        public string ToPlace { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Required]
        public string startTime { set; get; }
        [Required]
        public string EndTime { set; get; }
        [Required]
        public int ScheduledDays { get; set; }
        [Required]
        public string Instrument { get; set; }
        [Required]
        public int BClassCount { get; set; }
        [Required]
        public int BclassAvailCount { set; get; }
        [Required]
        public int NBClassCount { get; set; }
        [Required]
        public int NBclassAvailableCount { set; get; }
        [Required]
        public decimal TicketCost { get; set; }
        [Required]
        public decimal Discount { get; set; }
        [Required]
        public string? TicketCoopun { get; set; }
        [Required]
        public int Rows { get; set; }
        [Required]
        public int Meal { get; set; }
        [Required]
        public string CreatedBy { set; get; }
    }
}
