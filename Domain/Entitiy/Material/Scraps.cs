using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy.Material
{
    public class Scraps : EntityBase
    {
        public float Thickness { get; set; }
        public string? Color { get; set; }
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string? MaterialType { get; set; }
        public int? count { get; set; }

    }
}
