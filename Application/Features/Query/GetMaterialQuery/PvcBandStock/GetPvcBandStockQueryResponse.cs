using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.PvcBandStock
{
    public class GetPvcBandStockQueryResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
        public List<PvcBandDto>? PvcBand { get; set; }
    }
}
