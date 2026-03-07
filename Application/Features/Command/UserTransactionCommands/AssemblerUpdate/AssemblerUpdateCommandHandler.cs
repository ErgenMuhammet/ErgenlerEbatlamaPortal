using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Command.UserTransaction.AssemblerUpdate
{
    public class AssemblerUpdateCommandHandler : IRequestHandler<AssemblerUpdateCommandRequest, AssemblerUpdateCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppContext _context;
        public AssemblerUpdateCommandHandler(UserManager<AppUser> userManager, IAppContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<AssemblerUpdateCommandResponse> Handle(AssemblerUpdateCommandRequest request, CancellationToken cancellationToken)
        {

            var user = await _context.AppUsers.Include(x => x.Jobs).FirstOrDefaultAsync(x => x.Id.ToString() == request.UserIdentifier, cancellationToken);

            if (user == null)
            {
                return new AssemblerUpdateCommandResponse
                {
                    IsSucces = false,
                    Message = "Beklenmedik bir hata ile karşılaşıldı"
                };
            }

            user.Jobs.Experience = request.Experience;
            try
            {
                user.IsUpdated = true;
                await _context.SaveChangesAsync(cancellationToken);
                return new AssemblerUpdateCommandResponse
                {
                    IsSucces = true,
                    Message = "Bilgiler başarı ile kaydedildi."

                };
            }
            catch (Exception ex)
            {

                return new AssemblerUpdateCommandResponse
                {
                    IsSucces = false,
                    Message = $"Bilgiler kaydedilirken bir hata ile karşılaşıldı.Hata : {ex.Message}"
                }; ;
            }
          

        }
    }
}
