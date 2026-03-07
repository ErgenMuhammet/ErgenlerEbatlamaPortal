using Application.Features.Command.UserTransaction.PanelSawyerUpdate;
using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.CarpenterUpdate
{
    public class CarpenterUpdateCommandHandler : IRequestHandler<CarpenterUpdateCommandRequest, CarpenterUpdateCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppContext _context;

        public CarpenterUpdateCommandHandler(UserManager<AppUser> userManager, IAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<CarpenterUpdateCommandResponse> Handle(CarpenterUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _context.AppUsers.Include(x => x.Jobs).FirstOrDefaultAsync(x => x.Id.ToString() == request.UserIdentifier, cancellationToken);

            if (user == null)
            {
                return new CarpenterUpdateCommandResponse
                {
                    IsSucces = false,
                    Message = "Beklenmedik bir hata ile karşılaşıldı"
                };
            }

            user.Jobs.Experience = request.Experience;
            user.Jobs.AdressDescription = request.AdressDescription;
            user.Jobs.WorkShopName = request.WorkShopName;

            try
            {
                user.IsUpdated = true;
                await _context.SaveChangesAsync(cancellationToken);

                return new CarpenterUpdateCommandResponse
                {
                    IsSucces = true,
                    Message = "Bilgiler başarı ile kaydedildi."

                };
            }
            catch (Exception ex)
            {

                return new CarpenterUpdateCommandResponse
                {
                    IsSucces = false,
                    Message = $"Bilgiler kaydedilirken bir hata ile karşılaşıldı.Hata : {ex.Message}"
                };
            }
        }
    }
}
