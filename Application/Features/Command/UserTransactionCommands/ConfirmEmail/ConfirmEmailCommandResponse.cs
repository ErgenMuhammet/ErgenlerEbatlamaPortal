using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.ConfirmEmail
{
    public class ConfirmEmailCommandResponse
    {
        public string? Message { get; set; }
        public bool IsSucces { get; set; }
        public List<string>? Errors { get; set; }
    }
}
