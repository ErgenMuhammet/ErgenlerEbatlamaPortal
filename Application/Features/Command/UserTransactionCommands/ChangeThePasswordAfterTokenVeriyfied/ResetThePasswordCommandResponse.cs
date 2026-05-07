using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransactionCommands.ChangeThePasswordAfterTokenVeriyfied
{
    public class ResetThePasswordCommandResponse
    {
        public bool IsSuccess { get; set; }
        public string Message  { get; set; }

    }
}
