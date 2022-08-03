using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Airline.Inventory.ViewModel
{
    public class updaeAirlineViewModel
    {
        [Required]
        public int AirlineId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string UpdatedBy { set; get; }
    }
}
