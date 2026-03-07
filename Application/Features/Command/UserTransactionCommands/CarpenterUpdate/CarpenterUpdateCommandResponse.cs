using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.CarpenterUpdate
{
    public class CarpenterUpdateCommandResponse
    {
        public string UserId { get; set; }
        public string? Message { get; set; }
        public bool IsSucces { get; set; }
    }
}
