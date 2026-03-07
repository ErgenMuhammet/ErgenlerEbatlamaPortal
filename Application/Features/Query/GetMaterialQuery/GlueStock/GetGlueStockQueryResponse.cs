using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.GlueStock
{
    public class GetGlueStockQueryResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
        public List<GlueDto> Glue { get; set; }
    }
}
