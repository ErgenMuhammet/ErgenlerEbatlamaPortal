using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Command.UserTransaction.ValidatePasswordToken
{
    public class ValidatePasswordTokenCommandHandler : IRequestHandler<ValidatePasswordTokenCommandRequest, ValidatePasswordTokenCommandResponse>
    {
        private readonly UserManager<AppUser>? _userManager;

        public ValidatePasswordTokenCommandHandler(UserManager<AppUser>? userManager)
        {
            _userManager = userManager;
        }

        public async Task<ValidatePasswordTokenCommandResponse> Handle(ValidatePasswordTokenCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return new ValidatePasswordTokenCommandResponse
                {
                    IsSucces = false,
                    Message = "Kullanıcı bilgisi okunamadı. Token geçersiz veya süresi dolmuş. Daha sonra tekrar deneyiniz."
                };
            }
                

                var result = await _userManager.VerifyUserTokenAsync(user, 
                    _userManager.Options.Tokens.PasswordResetTokenProvider,
                    "ResetPassword", request.PasswordToken  );

                if (result)
                {
                    return new ValidatePasswordTokenCommandResponse
                    {
                        IsSucces = true,
                        Message = "Şifre sıfırlama sayfasına yönlendiriliyorsunuz."
                    };
                }

                return new ValidatePasswordTokenCommandResponse
                {
                    IsSucces = false,
                    Message = "Token geçersiz veya süresi dolmuş tekrar deneyiniz."
                };
        }
    }
}

