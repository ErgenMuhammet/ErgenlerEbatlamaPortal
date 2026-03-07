using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.ValidatePasswordToken
{
    public class ValidatePasswordTokenCommandResponse
    {
        public string? Message { get; set; }
        public bool IsSucces { get; set; }
    }
}
