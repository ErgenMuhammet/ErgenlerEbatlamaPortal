using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitiy
{
    public class ProfitLossSituation
    {
        public Guid Id { get; set; }
        public float TotalProfit { get; set; }
        public float TotalLoss { get; set; }
        public DateTime? Date  { get; set; }
        public AppUser? Owner { get; set; }
        public string OwnerId { get; set; }
        public float? LastSituation { get; set; }

        public float GetLastSituation()
        {
            return TotalProfit-TotalLoss;
        }
    }
}
