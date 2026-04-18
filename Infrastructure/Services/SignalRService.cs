using Application.Interface;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SignalRService : ISignalR
    {
        private readonly IHubContext<HubService> _hub;

        public SignalRService(IHubContext<HubService> hub)
        {
            _hub = hub;
        }

        public async Task IsDelivered(string SenderId, string MessageId)
        {
            await _hub.Clients.User(SenderId).SendAsync("IsDelivered", MessageId);
        }

        public async Task MarkAsRead(string ReceiverId, string SenderId)
        {
            await _hub.Clients.User(SenderId).SendAsync("MessageIsRead",ReceiverId);
        }

        public async Task SendMessage(string ReceiverId, string Message)
        {
            await _hub.Clients.User(ReceiverId).SendAsync("SendMessage",Message);
        }
    }
}
