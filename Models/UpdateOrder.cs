using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryAPI.Models
{
    public class UpdateOrder
    {
        public string Description { get; set; }
        public string Address { get; set; }

        [Required]
        public string UpdateBy { get; set; }
    }
}
