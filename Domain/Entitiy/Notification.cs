using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy
{
    public class Notification
    {
        public Guid Id { get; set; } = new Guid();
        public string? Content { get; set; }
        public AppUser Owner { get; set; }
        public string? OwnerId { get; set; }
        
    }
}
