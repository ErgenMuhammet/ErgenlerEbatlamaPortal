using Application.DTOs;
using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Query.GetMaterialQuery.GetMaterialStock
{
    public class GetMaterialStockQueryHandler : IRequestHandler<GetMaterialStockQueryRequest , GetMaterialStockQueryResponse<object>>
    {
        private readonly IAppContext _context;

        public GetMaterialStockQueryHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<GetMaterialStockQueryResponse<object>> Handle(GetMaterialStockQueryRequest request, CancellationToken cancellationToken)
        {
            dynamic? StockList = request.MaterialType?.ToLower().Trim() switch 
            {
                //object kelimesi bir nesne döneceğim demek
                "mdf" => await _context.Mdf.AsNoTracking().Where(x => x.OwnerID == request.OwnerId).Select(m => new MdfDto
                {
                    Brand = m.Brand,
                    Color = m.Color,
                    Stock = m.Stock,
                    Thickness = m.Thickness,
                    Weight = m.Weight

                }).Cast<object>().ToListAsync(cancellationToken),

                "glue" => await _context.Glue.AsNoTracking().Where(x => x.OwnerID == request.OwnerId).Select(m => new GlueDto
                {
                    Brand = m.Brand,
                    Weight = m.Weight,
                    Stock = m.Stock,

                }).Cast<object>().ToListAsync(cancellationToken),

                "backpanel" =>  await _context.BackPanel.AsNoTracking().Where(x => x.OwnerID == request.OwnerId).Select(m => new BackPanelDto
                {
                    Brand = m.Brand,
                    Color = m.Color,
                    Stock = m.Stock,
                    Thickness =m.Thickness,

                }).Cast<object>().ToListAsync(cancellationToken),

                "pvcband" =>  await _context.PvcBand.AsNoTracking().Where(x => x.OwnerID == request.OwnerId).Select(m => new PvcBandDto
                {
                    Brand = m.Brand,
                    Color = m.Color,
                    Stock = m.Stock,
                    Thickness = m.Thickness,

                }).Cast<object>().ToListAsync(cancellationToken),

                "invoice" => (object) await _context.Invoice.AsNoTracking().Where(x => x.OwnerId == request.OwnerId).Select(m => new InvoiceDto
                {
                    LastPaymentDate = m.LastPaymentDate,
                    Name = m.InvoiceNo,
                    Price = m.Cost

                }).Cast<object>().ToListAsync(cancellationToken),

                _ => null
            };

            if (StockList! == null)
            {
                return new GetMaterialStockQueryResponse<object>
                {
                    IsSucces = false,
                    List = null ,
                    Message = "Materyal türü bulunamadı"
                };
            }

            if (StockList.Count == 0)
            {
                return new GetMaterialStockQueryResponse<object>
                {
                    IsSucces = true,
                    List = null,
                    Message = $"{request.MaterialType} Listesi herhangi bir bilgi içermiyor"
                };
            }

            return new GetMaterialStockQueryResponse<object>
            {
                IsSucces = true,
                Message = $"{request.MaterialType} Listesi başarıyla getirildi",
                List = StockList!
            };
        }
    }
}
