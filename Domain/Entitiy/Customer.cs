


namespace Domain.Entitiy
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }      
        public string? Phone { get; set; }
        public List<Order> Orders { get; set; }
        public string? OrderId { get; set; }
        public bool IsActive { get; set; }
        



    }
}
