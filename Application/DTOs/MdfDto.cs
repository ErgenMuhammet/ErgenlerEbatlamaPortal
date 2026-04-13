using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class MdfDto
    {
        public float Thickness { get; set; }
        public string? Color { get; set; }
        public string? Brand { get; set; }
        public int Stock { get; set; }
        public float Weight { get; set; }
    }
}