using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.PasswordRecovery
{
    public class PasswordRecoveryCommandResponse
    {
        public string? Message { get; set; }
        public bool IsSucces { get; set; }
        
    }
}
