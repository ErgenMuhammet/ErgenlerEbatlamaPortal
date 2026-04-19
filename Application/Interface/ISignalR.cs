using Domain.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ISignalR
    {
        Task SendMessage(string ReceiverId, string Message);
        Task MarkAsRead(string ReceiverId , string SenderId);
        Task IsDelivered(string SenderId, string MessageId);
        Task SendNotification(string OwnerId , string Message);
    }
}
