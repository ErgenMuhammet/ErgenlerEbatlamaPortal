using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy
{
    public class Order
    {
        public Guid Id { get; set; }
        public string? OrderName { get; set; }
        public int? CountOfMdf { get; set; }
        public int? CountOfBackPanel { get; set; }
        public float? MetreOfPvcBand { get; set; }
        public float? CostOfOrder { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now.Date;
        public AppUser? Owner { get; set; }
        public string OwnerId { get; set; }
        public bool? IsDone { get; set; } = false;

    }
}
