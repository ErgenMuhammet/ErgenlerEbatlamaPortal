using Domain.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ExpenseDto
    {
        public string Id { get; set; }
        public DateTime? ExpenseDate { get; set; } = DateTime.Now.Date;
        public float? Amount { get; set; }
        public string? Description { get; set; }
    }
}
