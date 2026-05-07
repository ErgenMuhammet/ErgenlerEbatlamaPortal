using Application.Interface;
using Domain.Entitiy;
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
        private readonly IAppContext _context;
        public SignalRService(IHubContext<HubService> hub, IAppContext context )
        {
            _hub = hub;
            _context = context;
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
        public async Task SendNotification(string OwnerId, string Message)
        {
            await _hub.Clients.User(OwnerId).SendAsync("SendNotification" , Message);
        }
    }
}
