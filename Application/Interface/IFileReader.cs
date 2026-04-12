using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IFileReader
    {
        public Task<OrdersDto> ReadFile(string path,string OrderName);
        public Task<List<FileDetailDto>> ReadFile();

    }
}
