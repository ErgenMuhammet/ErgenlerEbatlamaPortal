using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy
{
    public class Income
    {
        public Guid Id { get; set; }
        public DateTime? IncomeDate { get; set; } = DateTime.Now.Date;

        public AppUser? Owner { get; set; }
        public string OwnerId { get; set; }

        public float Amount { get; set; }
        public string Description { get; set; }

        
    }
}
