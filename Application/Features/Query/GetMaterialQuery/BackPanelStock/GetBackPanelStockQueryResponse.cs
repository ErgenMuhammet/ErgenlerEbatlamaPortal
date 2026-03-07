using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.BackPanelStock
{
    public class GetBackPanelStockQueryResponse
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
        public List<BackPanelDto> BackPanel  { get; set; }
    }
}
