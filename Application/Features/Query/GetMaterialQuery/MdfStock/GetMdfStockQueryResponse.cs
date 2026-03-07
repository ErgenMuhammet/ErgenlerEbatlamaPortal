using Application.DTOs;
using Domain.Entitiy.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.MdfStock
{
    public class GetMdfStockQueryResponse 
    {
        public bool IsSucces { get; set; }
        public string? Message { get; set; }
        public List<MdfDto> Mdf { get; set; }
    }
}
