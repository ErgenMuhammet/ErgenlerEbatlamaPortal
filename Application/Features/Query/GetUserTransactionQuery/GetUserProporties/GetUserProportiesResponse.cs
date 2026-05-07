using Application.DTOs;
using Domain.Entitiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetUserProporties
{
    public class GetUserProportiesResponse
    {
        public string Message{ get; set; }
        public bool IsSucces { get; set; }
        public UserProportiesDto User { get; set; }
    }
}
