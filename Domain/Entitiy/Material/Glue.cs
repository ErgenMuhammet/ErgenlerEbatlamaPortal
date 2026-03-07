using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy.Material
{
    public class Glue : EntityBase
    {
        public float Weight { get; set; }
        public float Profit { get; set; }

      

        public float GetProfit() => Price - Cost;
    }
}
