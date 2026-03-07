//using Application.Interface;
//using Domain.Entitiy;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Features.Command.UserTransaction.PanelSawyerUpdate
//{
//    public class PanelSawyerUpdateCommandHandler : IRequestHandler<PanelSawyerUpdateCommandRequest, PanelSawyerUpdateCommandResponse>
//    {
//        private readonly IAppContext _context;
//        private readonly UserManager<AppUser> _userManager;
//        public PanelSawyerUpdateCommandHandler(IAppContext context, UserManager<AppUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }
//        public async Task<PanelSawyerUpdateCommandResponse> Handle(PanelSawyerUpdateCommandRequest request, CancellationToken cancellationToken)
//        {
//            var user = await _context.PanelSawyers.FindAsync(request.UserId,cancellationToken);

//            if (user == null)
//            {
//                return new PanelSawyerUpdateCommandResponse
//                {
//                    IsSucces = false,
//                    Message = "Beklenmedik bir hata ile karşılaşıldı"
//                };
//            }

//            user.Experience = request.Experience;
//            user.AdressDescription = request.AdressDescription;
//            user.WorkShopName = request.WorkShopName;
//            var result = await _context.SaveChangesAsync(cancellationToken);

//            if (result > 0)
//            {
//                return new PanelSawyerUpdateCommandResponse
//                {
//                    IsSucces = true,
//                    Message = "Bilgiler başarı ile kaydedildi."

//                };
//            }
//            else
//            {
//                return new PanelSawyerUpdateCommandResponse
//                {
//                    IsSucces = false,
//                    Message = "Bilgiler kaydedilirken bir hata ile karşılaşıldı."
//                };
//            }
//        }
//    }
//}
