using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class OrdersDto
    {
        public string? OrderName { get; set; }
        public int? CountOfMdf { get; set; }
        public int? CountOfBackPanel { get; set; }
        public float? MetreOfPvcBand { get; set; }
        public float? CostOfOrder { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now.Date;
    }
}
