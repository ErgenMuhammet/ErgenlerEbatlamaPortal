using Domain.GlobalEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy
{
    public class BaseJobs
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string WorkShopName { get; set; }
        public string AdressDescription { get; set; }
        public AppUser? User { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public int Experience { get; set; }
        public Category? Category { get; set; }
        public string? MastersName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
