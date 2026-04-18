using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; } = DateTime.Parse(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        public bool IsRead { get; set; } = false;
        public bool IsDelivered { get; set; } = false;
        public AppUser? Sender{ get; set; }
        public string SenderId { get; set; }
        public AppUser? Receiver { get; set; }
        public string ReceiverId { get; set; }
    }
}
