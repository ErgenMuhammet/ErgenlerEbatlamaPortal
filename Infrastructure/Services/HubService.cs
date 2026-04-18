using Application.Interface;
using Domain.Entitiy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class HubService : Hub
    {
        private readonly IAppContext _context;
        private readonly ISignalR _signalR;
        public HubService(IAppContext context, ISignalR signalR)
        {
            _context = context;           
            _signalR = signalR;
        }
        public async Task SendMessage(string ReceiverId, string content)
        {
            var ct = Context.ConnectionAborted;
            var SenderId = Context.UserIdentifier;

            var Message = new ChatMessage
            {
                Content = content,
                IsRead = false,
                ReceiverId = ReceiverId,
                SenderId = SenderId,
            };

            try
            {
                await _context.Messages.AddAsync(Message);
                await _context.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                throw new Exception($"Mesaj bilgisi veritabanına kaydedilirken bir hata oluştu. Hata : {ex.Message}");
            }

            await Clients.User(ReceiverId)
                        .SendAsync("ReceiveMessage", new
                        {
                            Message.Id,
                            SenderId,
                            content,
                            Message.SendAt
                        }); // geçici bir mesaj nesnesi olusturup gönderiyorum
        }

        public override async Task OnConnectedAsync()
        {
            var UserId = Context.UserIdentifier;
            var ct = Context.ConnectionAborted;
            var Messages = await _context.Messages.Where(x => x.ReceiverId == UserId && !x.IsRead).ToListAsync();

            foreach ( var message in Messages)
            {
                await Clients.User(UserId).SendAsync("ReceiveMessage", new ChatMessage
                {
                    Content = message.Content,
                    SendAt = message.SendAt,
                    SenderId = message.SenderId,
                },ct);

                message.IsDelivered = true;

                await Clients.Caller.SendAsync(message.SenderId, message.Id.ToString()); // mesajı gönderene ulaştı bilgisi için frontend kısmını tetikliyoruz
            }

            await _context.SaveChangesAsync(ct);
            await base.OnConnectedAsync();
        }

        public async Task MarkAsRead(string SenderId)
        {
            var UserId = Context.UserIdentifier;
            var ct = Context.ConnectionAborted;
            var Messages = await _context.Messages.Where(x => x.SenderId == SenderId && x.ReceiverId == UserId && !x.IsRead ).ToListAsync(ct);

            if (!Messages.Any())
            {
                return;
            }

            Messages.ForEach(x => x.IsRead = true); 

            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (Exception)
            {

                throw new Exception("Mesaj görüldü bilgisi işlenirken bir hata ile karşılaşıldı");
            }

            await Clients.User(SenderId).SendAsync("MarkAsRead" , UserId, ct);

        }
    }
}
