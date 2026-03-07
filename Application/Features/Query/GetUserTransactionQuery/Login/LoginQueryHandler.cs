using Application.Interface;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;


namespace Application.Features.Query.GetUserTransactionQuery.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQueryRequest, LoginQueryResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        public LoginQueryHandler(UserManager<AppUser> userManager, ITokenService tokenService) 
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<LoginQueryResponse> Handle(LoginQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if(user == null)
            {
                throw new AuthenticationException("Kullanıcı bulunamadı.Lütfen email adresinizi kontrol ediniz");
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!checkPassword)
            {
                throw new AuthenticationException("Kullanıcı adı veya şifre hatalıdır tekrar deneyiniz");
            }

            var token = await _tokenService.CreateToken(user);

            if (!user.EmailConfirmed)
            {
                throw new AuthenticationException("Giriş yapabilmek için lütfen email adresinize gelen doğrulama linkine tıklayınız");
            }

            return new LoginQueryResponse
            {
                Message = "Giriş başarılı",
                IsSucces = true,
                Expiration = DateTime.Now.AddMinutes(45),
                Token = token,
                UserId = user.Id,
                Job = user.UserCategory
            };
        }
    }
}
