using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Airline.Inventory.ViewModel
{
    public class airlineViewModel
    {
        [Required]
        public string AirlineName { set; get; }
        [Required]
        public string Address { set; get; }
        [Required]
        public string Contact { set; get; }
        [Required]
        public string CreatedBy { set; get; }
    }
}
