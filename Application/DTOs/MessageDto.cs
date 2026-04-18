using Domain.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class MessageDto
    {
        public string Content { get; set; }
        public DateTime SendAt { get; set; } = DateTime.Parse(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        public string SenderId { get; set; }
    }
}
