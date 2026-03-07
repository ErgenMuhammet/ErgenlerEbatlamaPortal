using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.Register
{
    public class RegisterCommandResponse
    {
        public string? Message { get; set; }
        public bool IsSucces { get; set; }
        public string? Errors { get; set; }
    }
}
