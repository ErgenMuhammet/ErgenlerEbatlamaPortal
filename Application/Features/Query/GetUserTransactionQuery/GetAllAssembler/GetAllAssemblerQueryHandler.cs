using Application.DTOs;
using Application.Interface;
using Domain.GlobalEnum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetUserTransactionQuery.GetAllAssembler
{
    public class GetAllAssemblerQueryHandler : IRequestHandler<GetAllAssemblerQueryRequest, GetAllAssemblerQueryResponse>
    {
        private readonly IAppContext _context;

        public GetAllAssemblerQueryHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<GetAllAssemblerQueryResponse> Handle(GetAllAssemblerQueryRequest request, CancellationToken cancellationToken)
        {
            List<AssemblerDto> Assemblers = await _context.
                Jobs.
                Where(x => x.Category == Category.Assembler).
                Select(x => new AssemblerDto
                {
                    MastersNName = x.MastersName,
                    Experience = x.Experience,
                    PhoneNumber = x.PhoneNumber,
                }).ToListAsync(cancellationToken);

            if (Assemblers == null)
            {
                return new GetAllAssemblerQueryResponse
                {
                    Assemblers =  null,
                    IsSuccess = false,
                    Message = "Montajcı listesi hazırlanırken bir hata oluştu"
                };
            }

            if (Assemblers.Count > 0)
            {
                return new GetAllAssemblerQueryResponse
                {
                    Assemblers = Assemblers,
                    IsSuccess = true,
                    Message = "Montajcı listesi başarıyla getirildi"
                };         
            }
            else 
            {
                return new GetAllAssemblerQueryResponse
                {
                    Assemblers = Assemblers,
                    IsSuccess = true,
                    Message = "Montajcı listesinde görüntülenecek kullanıcı yok."
                };
            }
        }
    }
}
