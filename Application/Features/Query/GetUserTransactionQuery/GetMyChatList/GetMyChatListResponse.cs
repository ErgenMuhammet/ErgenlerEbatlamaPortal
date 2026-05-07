using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetMyChatList
{
    public class GetMyChatListResponse
    {
        public List<MessageListDto> ChatList { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
