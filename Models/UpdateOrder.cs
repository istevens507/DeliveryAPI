using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryAPI.Models
{
    public class UpdateOrder
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public string UpdateBy { get; set; }
    }
}
