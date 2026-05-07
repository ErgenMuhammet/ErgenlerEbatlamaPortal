using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransactionCommands.ChangeThePasswordAfterTokenVeriyfied
{
    public class ResetThePasswordCommandHandler : IRequestHandler<ResetThePasswordCommandRequest, ResetThePasswordCommandResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public ResetThePasswordCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ResetThePasswordCommandResponse> Handle(ResetThePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return new ResetThePasswordCommandResponse
                {
                    IsSuccess = false,
                    Message = "Kullanıcı bilgisine ulaşılamadı."
                };
            }

            if (request.Password != request.PasswordConfirm)
            {
                return new ResetThePasswordCommandResponse
                {
                    IsSuccess = false,
                    Message = "Şifreler uyuşmuyor."
                };
            }

            try
            {
                var decodedBytes = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(request.Token);
                var originalToken = System.Text.Encoding.UTF8.GetString(decodedBytes);
                var identityResult = await _userManager.ResetPasswordAsync(user, originalToken, request.Password);
                if (!identityResult.Succeeded)
                {
                    return new ResetThePasswordCommandResponse
                    {
                        IsSuccess = false,
                        Message = string.Join(", ", identityResult.Errors.Select(e => e.Description))
                    };
                }
            }
            catch (Exception ex)
            {

                return new ResetThePasswordCommandResponse
                {
                    IsSuccess = false,
                    Message = $"Kullanıcı şifresi sıfırlanırken bir hata ile karşılaşıldı. Hata {ex.Message}.",
                };
            }

            return new ResetThePasswordCommandResponse
            {
                IsSuccess = true,
                Message = "Şifre başarıyla sıfırlandı. Yeni şifreniz ile giriş sağlayabilirsiniz."
            };
        }
    }
}
