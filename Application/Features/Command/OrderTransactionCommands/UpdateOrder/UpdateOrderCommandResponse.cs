using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.OrderTransactionCommands.UpdateOrder
{
    public class UpdateOrderCommandResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
