using Application.Features.Command.OrderTransactionCommands.AddOrder;
using Application.Features.Command.OrderTransactionCommands.DeleteOrder;
using Application.Features.Command.OrderTransactionCommands.OrderIsDone;
using Application.Features.Command.OrderTransactionCommands.UpdateOrder;
using Application.Features.Query.OrderTransaction.GetAllMeasurements;
using Application.Features.Query.OrderTransaction.GetAllOrdersList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ErgenlerEbatlamaPortalAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("/portal/order")]
    public class OrderController : ControllerBase
    {
        protected string OwnerId => User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier)!;
        private readonly IMediator _mediatR;
        public OrderController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }


        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderCommandRequest request)
        {
            request.OwnerId = OwnerId;
            
            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }


        [HttpGet("GetAllOrder")]
        public async Task<IActionResult> GetAllOrder()
        {
            var request = new GetAllOrdersListRequest();
           
            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpDelete("Delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] string orderId) 
        {
            var request = new DeleteOrderCommandRequest();

            request.OwnerId = OwnerId;
            request.OrderId= orderId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("Update/{orderId}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] string orderId, [FromBody] UpdateOrderCommandRequest request)
        {
            request.OwnerId = OwnerId;
            request.OrderId = orderId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [HttpPatch("OrderIsDone/{orderId}")]
        public async Task<IActionResult> OrderIsDone([FromRoute] string orderId )
        {
            var request = new OrderIsDoneCommandRequest();

            request.OwnerId = OwnerId;
            request.OrderId = orderId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [HttpGet("GetMeasurements")]
        public async Task<IActionResult> GetTheMeasurementForSelectedCostumer([FromQuery] GetAllMeasurementsQueryRequest request)
        {
            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
