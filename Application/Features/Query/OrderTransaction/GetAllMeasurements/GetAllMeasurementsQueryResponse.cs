using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.OrderTransaction.GetAllMeasurements
{
    public class GetAllMeasurementsQueryResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public OrdersDto? Orders { get; set; }
        public List<MdfProportiesDTO>? MdfProporties { get; set; }
    }
}
