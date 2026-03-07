using Domain.GlobalEnum;

namespace Domain.Entitiy
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public string? InvoiceNo { get; set; }
        public string? InvoiceName { get; set; }
        public float Cost { get; set; }
        public DateTime? LastPaymentDate { get; set; } 
        public AppUser? Owner { get; set; }
        public string OwnerId { get; set; }

        public bool? BeenPaid { get; set; } = false;

    }
}
