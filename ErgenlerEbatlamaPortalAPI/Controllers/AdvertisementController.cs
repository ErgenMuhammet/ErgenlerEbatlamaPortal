using Application.Features.Command.AdvertisementTransactionHandlers.AddAdvertisement;
using Application.Features.Command.AdvertisementTransactionHandlers.DeleteAdvertisement;
using Application.Features.Command.AdvertisementTransactionHandlers.UpdateAdvertisement;
using Application.Features.Query.GetAdvertisement;
using Application.Features.Query.GetAllAdvertisementQuery;
using Application.Features.Query.GetMyPastAdvertisement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ErgenlerEbatlamaPortalAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/portal")]
    public class AdvertisementController : ControllerBase
    {
       protected string UserId => User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;

        private readonly IMediator _mediatR;

        public AdvertisementController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPatch("UpdateAdvertisement")]
        public async Task<IActionResult> UpdateAdvs([FromBody] UpdateAdvertisementCommandRequest request)
        {
            request.OwnerId = UserId;   

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

        [HttpDelete("DeleteAdvertisement")]
        public async Task<IActionResult> DeleteAdvs()
        {
            var request = new DeleteAdvertismentCommandRequest();          

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("GetAdvertisement/{id}")]
        public async Task<IActionResult> GetAdvs([FromRoute] GetAdvertisementQueryRequest request) 
        {
            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetAllAdvertisement")]
        public async Task<IActionResult> GetAllAdvs()
        {
            var request = new GetAllAdvertisementQueryRequest();

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetMyAdvertisement")]
        public async Task<IActionResult> GetMyAdvs()
        {
            var request = new GetMyPastAdvertisementQueryRequest();

            request.OwnerId = request.OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("AddAdvertisement")]
        public async Task<IActionResult> AddAdvs([FromBody] AddAdvertisementsCommandRequest request)
        {
            request.OwnerId = request.OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
