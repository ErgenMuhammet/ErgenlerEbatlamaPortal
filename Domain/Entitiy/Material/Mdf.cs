using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy.Material
{
    public class Mdf : EntityBase
    {
        public float Thickness { get; set; }
        public string? Color { get; set; }     
        public float Profit { get; set; }
        public int Weight { get; set; }
        
       

    }
}
