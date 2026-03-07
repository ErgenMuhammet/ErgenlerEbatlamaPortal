using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetAllCarpenters
{
    public class GetAllCarpentersQueryResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<CarpenterDto>? Carpenters { get; set; }
    }
}
