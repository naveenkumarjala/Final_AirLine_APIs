using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Airline.Inventory.ViewModel
{
    public class CancelAirlineViewModel
    {
        [Required]
        public int airlineId { set; get; }
        [Required]
        public string updatedBy { set; get; }
    }
}
