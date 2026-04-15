using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SawnPieceForOrders
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public int Count { get; set; }
        public bool LongSide1 { get; set; }
        public bool LongSide2 { get; set; }
        public bool ShortSide1 { get; set; } 
        public bool ShortSide2 { get; set; }
    }
}
