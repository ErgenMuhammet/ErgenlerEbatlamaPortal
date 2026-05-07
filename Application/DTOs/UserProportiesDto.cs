using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserProportiesDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WorkShopName { get; set; }
        public string? AdressDescription { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Experience { get; set; }
        public string? WorkPhoneNumber { get; set; }
        public string? City { get; set; }   

    }
}
