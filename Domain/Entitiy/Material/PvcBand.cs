using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy.Material
{
    public class PvcBand : EntityBase
    {
        public float Thickness { get; set; }
        public string? Color { get; set; }
        public float? Profit { get; set; }
        public int Length { get; set; }


        public float GetProfit() => Length / Cost * Price;
    }
}
