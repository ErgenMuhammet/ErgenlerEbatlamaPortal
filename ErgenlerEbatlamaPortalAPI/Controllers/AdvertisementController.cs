using Application.Features.Command.AdvertisementTransactionHandlers.AddAdvertisement;
using Application.Features.Command.AdvertisementTransactionHandlers.DeleteAdvertisement;
using Application.Features.Command.AdvertisementTransactionHandlers.OfferToAdvertisement;
using Application.Features.Command.AdvertisementTransactionHandlers.UpdateAdvertisement;
using Application.Features.Query.GetAdvertisement;
using Application.Features.Query.GetAllAdvertisementQuery;
using Application.Features.Query.GetUserTransactionQuery.GetMyPastAdvertisement;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ErgenlerEbatlamaPortalAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("portal/")]
    public class AdvertisementController : ControllerBase
    {
       protected string UserId => User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;

        private readonly IMediator _mediatR;

        public AdvertisementController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPatch("UpdateAdvertisement")]
        public async Task<IActionResult> UpdateAdvs([FromBody] UpdateAdvertisementCommandRequest request , string AdvertisementId)
        {
            request.OwnerId = UserId;   
            request.AdvertisementId = AdvertisementId;
            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

        [HttpDelete("DeleteAdvertisement/{id}")]
        public async Task<IActionResult> DeleteAdvs([FromRoute] string id)
        {
            var request = new DeleteAdvertismentCommandRequest();

            request.AdvertisementId = id;

            request.OwnerId = UserId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetAdvertisement/{id}")]
        public async Task<IActionResult> GetAdvs([FromRoute] string id ) 
        {
            var request = new GetAdvertisementQueryRequest();

            request.AdvertismentId = id ;

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

            request.UserId = UserId;

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

            request.OwnerId = UserId;

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
            request.OwnerId = UserId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("OfferToAdvertisement")]
        public async Task<IActionResult> OfferToAdvertisement([FromRoute] OfferToAdvertisementCommandRequest request)
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
