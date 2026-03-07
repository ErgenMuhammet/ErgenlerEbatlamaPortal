using Application.Features.Command.UserTransaction.ConfirmEmail;
using Application.Features.Command.UserTransaction.AssemblerUpdate;
using Application.Features.Command.UserTransaction.CarpenterUpdate;
using Application.Features.Command.UserTransaction.PanelSawyerUpdate;
using Application.Features.Command.UserTransaction.PasswordRecovery;
using Application.Features.Command.UserTransaction.Register;
using Application.Features.Command.UserTransaction.ValidatePasswordToken;
using Azure.Core;
using Domain.Entitiy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Features.Query.GetUserTransactionQuery.Login;

namespace ErgenlerEbatlamaPortalAPI.Controllers
{
   
    [ApiController]
    [Route("portal/")]
    public class UserTransactionsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediatR;
        protected string OwnerId => User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;
        public UserTransactionsController(UserManager<AppUser> userManager, IMediator mediatR)
        {
            _userManager = userManager;
            _mediatR = mediatR;
        }

        [HttpPost("login")]        
        public async Task<IActionResult> Login([FromBody] LoginQueryRequest request)
        {
            var result = await _mediatR.Send(request);

            if (result.IsSucces)
            {
                return Ok(result);
            }

            return BadRequest("Kullanıcı adı veya şifre hatalı");

        }
        
        [HttpPost("KayıtOl")]
        public async Task<IActionResult> Register([FromBody] RegisterCommandRequest request)
        {
            var result = await _mediatR.Send(request);

            if (!result.IsSucces) 
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("Marangoz")]
        public async Task<IActionResult> Carpenter([FromBody] CarpenterUpdateCommandRequest request)
        {
            
            request.UserIdentifier = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
             return BadRequest(result);
            

            return Ok(result);
        }

        [Authorize]
        [HttpPost("Ebatlamacı")]
        public async Task<IActionResult> PanelSawyer([FromBody] PanelSawyerUpdateCommandRequest request)
        {
            request.UserIdentifier = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("Montajcı")]
        public async Task<IActionResult> Assebmler([FromBody] AssemblerUpdateCommandRequest request)
        {
            request.UserIdentifier = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
                return BadRequest(result);

            return Ok(result);
        }
    
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userid, string token)
        {
            var request = new ConfirmEmailCommandRequest
            {
                UserId = userid,
                Token = token,
            };
            var response = await _mediatR.Send(request);

            if (response.IsSucces)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPatch("PasswordRecovery")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordRecoveryCommandRequest request)
        {
           
            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("ValidateToken")]
        public async Task<IActionResult> ValidateToken([FromQuery] string userId, [FromQuery] string token, [FromBody] ValidatePasswordTokenCommandRequest request)
        {
            request.userId = userId;
            request.passwordToken = token;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
                return BadRequest(result);

            return Ok(result);
        }      
        
    }


}

