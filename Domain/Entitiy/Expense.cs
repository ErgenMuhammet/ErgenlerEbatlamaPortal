
namespace Domain.Entitiy
{
    public class Expense
    {
        public Guid Id { get; set; }
        public DateTime? ExpenseDate { get; set; } = DateTime.Now.Date;

        public AppUser? Owner { get; set; }
        public string OwnerId { get; set; }

       

        public float Amount { get; set; }
        public string Description { get; set; }


    }
}
