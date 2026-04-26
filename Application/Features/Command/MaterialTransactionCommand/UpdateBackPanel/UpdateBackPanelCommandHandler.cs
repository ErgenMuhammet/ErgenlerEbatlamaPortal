using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.MaterialTransactionCommand.UpdateBackPanel
{
    public class UpdateBackPanelCommandHandler : IRequestHandler<UpdateBackPanelCommandRequest, UpdateBackPanelCommandResponse>
    {
        private readonly IAppContext _context;

        public UpdateBackPanelCommandHandler(IAppContext context)
        {
            _context = context;
        }

        public async Task<UpdateBackPanelCommandResponse> Handle(UpdateBackPanelCommandRequest request, CancellationToken cancellationToken)
        {
            var backpanel = await _context.BackPanel.FirstOrDefaultAsync(x => x.Id.ToString() == request.BackPanelId);

            if (backpanel == null)
            {
                return new UpdateBackPanelCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili arkalık bilgisine ulaşılamadı"
                };
            }

            if (backpanel!.OwnerID != request.OwnerId)
            {
                return new UpdateBackPanelCommandResponse
                {
                    IsSucces = false,
                    Message = "İlgili arkalık mevcut kullanıcıya ait değildir."
                };
            }

            try
            {
                var IsContain = await _context.
                    BackPanel.
                    FirstOrDefaultAsync(x => x.OwnerID  == request.OwnerId &&
                    x.Id.ToString() != request.BackPanelId &&
                    x.Color == request.Color &&
                    x.Thickness == request.Thickness && 
                    x.Brand == request.Brand);

                if (IsContain != null)
                {
                    IsContain.Stock += request.Stock;

                    _context.BackPanel.Remove(backpanel);

                    await _context.SaveChangesAsync(cancellationToken);

                    return new UpdateBackPanelCommandResponse
                    {
                        IsSucces = true,
                        Message = "Listenizde böyle bir ürün bulunduğu için güncellenen ürün stoğa eklendi"
                    };
                }
                else
                {
                    backpanel.Thickness = request.Thickness;
                    backpanel.Stock = request.Stock;
                    backpanel.Color = request.Color;
                    backpanel.Brand = request.Brand;

                    await _context.SaveChangesAsync(cancellationToken);

                    return new UpdateBackPanelCommandResponse
                    {
                        IsSucces = false,
                        Message = "Bilgiler başarıyla güncellendi"
                    };

                }
            }
            catch (Exception ex)
            {

                return new UpdateBackPanelCommandResponse
                {
                    IsSucces = false,
                    Message = $"Bilgiler güncellenirken bir hatayla karşılaşıldı.Hata : {ex.Message}"
                };
            }


        }
    }
}
