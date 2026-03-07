using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enums = Domain.GlobalEnum.Category;

namespace Domain.Entitiy.Material
{
    public class EntityBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedTime { get; set; } = DateTime.Now.Date;
        public string? Brand { get; set; }
        public int Stock { get; set; }

        public AppUser Owner { get; set; }
        public string? OwnerID { get; set; }
        
        public float Cost { get; set; }
        public float Price { get; set; }

    }
}
