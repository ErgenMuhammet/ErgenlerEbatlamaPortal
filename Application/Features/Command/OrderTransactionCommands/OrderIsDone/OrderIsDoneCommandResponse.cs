using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.OrderTransactionCommands.OrderIsDone
{
    public class OrderIsDoneCommandResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
