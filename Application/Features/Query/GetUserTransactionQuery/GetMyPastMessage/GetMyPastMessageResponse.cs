using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetMyPastMessage
{
    public class GetMyPastMessageResponse
    {
        public string? Message{ get; set; }
        public bool IsSuccess { get; set; }
        public List<MessageDto> Messages { get; set; }
    }
}
