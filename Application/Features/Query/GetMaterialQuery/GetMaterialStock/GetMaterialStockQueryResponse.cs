using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.GetMaterialStock
{
    public class GetMaterialStockQueryResponse<T>
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
        public List<T> List { get; set; }
    }
}
