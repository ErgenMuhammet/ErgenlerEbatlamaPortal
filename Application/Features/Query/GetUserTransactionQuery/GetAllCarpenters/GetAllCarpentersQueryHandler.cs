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

namespace Application.Features.Query.GetUserTransactionQuery.GetAllCarpenters
{

    public class GetAllCarpentersQueryHandler : IRequestHandler<GetAllCarpentersQueryRequest, GetAllCarpentersQueryResponse>
    {
        private readonly IAppContext _context;

        public GetAllCarpentersQueryHandler(IAppContext context)
        {
            _context = context;
        }
        public async Task<GetAllCarpentersQueryResponse> Handle(GetAllCarpentersQueryRequest request, CancellationToken cancellationToken)
        {

            List<CarpenterDto> Carpenters = await _context.
                Jobs.
                Where(x => x.Category == Category.Carpenter).
                Select(x => new CarpenterDto
                {
                    AdressDescription = x.AdressDescription,
                    Experience = x.Experience,
                    WorkShopName = x.WorkShopName,
                    CarpentersName = x.MastersName,
                }).ToListAsync(cancellationToken);

            if (Carpenters == null)
            {
                return new GetAllCarpentersQueryResponse
                {
                    Carpenters = null,
                    Message = "Marangoz listesi getirilirken bir problem ile karşılaşıldı",
                    IsSuccess = false
                };
            }


            if (Carpenters != null && Carpenters.Count > 0)
            {
                return new GetAllCarpentersQueryResponse
                {
                    Carpenters = Carpenters,
                    IsSuccess = true,
                    Message = "Marangoz listesi getirildi."
                };
            }
            else
            {
                return new GetAllCarpentersQueryResponse
                {
                    Carpenters = Carpenters,
                    IsSuccess = true,
                    Message = "Marangoz listesinde görüntülenecek veri bulunamamaktadır."
                };
            }
        }
    }
}
