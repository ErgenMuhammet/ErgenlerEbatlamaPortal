using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.OrderTransaction.GetAllOrdersList
{
    public class GetAllOrdersListResponse 
    {
        public List<FileDetailDto>? FileDetails{ get; set; }
        public string? Message { get; set; } 
        public bool IsSuccess { get; set; } 
    }
}
