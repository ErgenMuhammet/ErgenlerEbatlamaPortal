using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.OrderTransaction.GetAllMeasurements
{
    public class GetAllMeasurementsQueryRequest : IRequest<GetAllMeasurementsQueryResponse>
    {
        public string? Path { get; set; }
        public string? OrderName { get; set; }
    }
}
