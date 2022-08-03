using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Airline.Inventory.ViewModel
{
    public class cancelViewModel
    {
        [Required]
        public string FlightNumber { set; get; }
        [Required]
        public string updatedby { set; get; }
    }
}
