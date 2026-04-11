using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class OrdersDto
    {
        public string? CustomerName { get; set; }
        public int? CountOfMdf { get; set; }
        public int? CountOfBackPanel { get; set; }
        public float? MetreOfPvcBand { get; set; }     
        public DateTime OrderDate { get; set; }
        public List<SawnPieceForOrders> SawnPiece { get; set; }
        public  float CostOfOrder{ get; set; }
    }
}
