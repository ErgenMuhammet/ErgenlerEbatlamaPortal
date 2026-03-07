using Domain.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CarpenterDto
    {
        public string? CarpentersName { get; set; }
        public string? WorkShopName { get; set; }
        public string? AdressDescription { get; set; }
        public int Experience { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
