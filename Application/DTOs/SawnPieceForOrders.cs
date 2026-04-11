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
        public bool WidhtBand1 { get; set; }
        public bool WidhtBand2 { get; set; }
        public bool HeightBand1 { get; set; }
        public bool HeightBand2 { get; set; }
        public int Count { get; set; }
    }
}
