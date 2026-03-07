using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy.Material
{
    public class BackPanel : EntityBase
    {
        public float Thickness { get; set; }
        public string? Color { get; set; }
        public float Profit { get; set; }

        public float GetProfit() => Price - Cost;
    }
}
