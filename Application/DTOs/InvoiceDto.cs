using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class InvoiceDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public float? Price { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public string? InvoicesNo { get; set; }

    }
}
