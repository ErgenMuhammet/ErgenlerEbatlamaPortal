using Application.DTOs;
using Domain.Entitiy.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.GetScraps
{
    public class GetAllScrapsQueryResponse
    {        
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
        public List<ScrapsDto> Scraps { get; set; }
    }
}
