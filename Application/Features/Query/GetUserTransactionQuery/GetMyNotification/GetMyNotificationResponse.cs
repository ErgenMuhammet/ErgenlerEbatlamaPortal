using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetMyNotification
{
    public class GetMyNotificationResponse
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public List<NotificationDto> Notifications { get; set; }
    }
}
