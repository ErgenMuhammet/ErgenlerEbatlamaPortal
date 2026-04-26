using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PvcBandDto
    {
        public string Id { get; set; }
        public string? Brand { get; set; }
        public float Thickness { get; set; }
        public string? Color { get; set; }
        public int Stock { get; set; }

    }
}
