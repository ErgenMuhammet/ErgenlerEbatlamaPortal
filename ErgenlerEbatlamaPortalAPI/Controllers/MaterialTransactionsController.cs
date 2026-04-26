using Application.Features.Command.MaterialTransactionCommand.AddBackPanel;
using Application.Features.Command.MaterialTransactionCommand.AddGlue;
using Application.Features.Command.MaterialTransactionCommand.AddMdf;
using Application.Features.Command.MaterialTransactionCommand.AddPvcBand;
using Application.Features.Command.MaterialTransactionCommand.AddScrap;
using Application.Features.Command.MaterialTransactionCommand.ReduceBackPanel;
using Application.Features.Command.MaterialTransactionCommand.ReduceGlue;
using Application.Features.Command.MaterialTransactionCommand.ReduceMdf;
using Application.Features.Command.MaterialTransactionCommand.ReducePvcBand;
using Application.Features.Command.MaterialTransactionCommand.ReduceScrap;
using Application.Features.Command.MaterialTransactionCommand.UpdateBackPanel;
using Application.Features.Command.MaterialTransactionCommand.UpdateGlue;
using Application.Features.Command.MaterialTransactionCommand.UpdateMdf;
using Application.Features.Command.MaterialTransactionCommand.UpdatePvcBand;
using Application.Features.Command.MaterialTransactionCommand.UpdateScrap;
using Application.Features.Query.GetMaterialQuery.BackPanelStock;
using Application.Features.Query.GetMaterialQuery.GetScraps;
using Application.Features.Query.GetMaterialQuery.GlueStock;
using Application.Features.Query.GetMaterialQuery.MdfStock;
using Application.Features.Query.GetMaterialQuery.PvcBandStock;
using Domain.Entitiy.Material;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ErgenlerEbatlamaPortalAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("portal/")]
    public class MaterialTransactionsController : ControllerBase
    {
        private readonly IMediator _mediatR;
        protected string OwnerId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public MaterialTransactionsController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpGet("Stok/Mdf")]
        public async Task<IActionResult> GetMdfStock()
        {
            var request = new GetMdfStockQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("Stok/Glue")]
        public async Task<IActionResult> GetGlueStock()
        {
            var request = new GetGlueStockQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("Stok/BackPanel")]
        public async Task<IActionResult> GetBackPanelStock()
        {
            var request = new GetBackPanelStockQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("Stok/Scraps")]
        public async Task<IActionResult> GetScraps()
        {
            var request = new GetAllScrapsQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("Stok/PvcBand")]
        public async Task<IActionResult> GetPvcBandStock()
        {
            var request = new GetPvcBandStockQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpPost("Stok/AddPvcBand")]
        public async Task<IActionResult> AddPvcBand([FromBody] AddPvcBandCommandRequest request )
        {           
            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Stok/AddGlue")]
        public async Task<IActionResult> AddGlue([FromBody] AddGlueCommandRequest request)
        {
            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Stok/AddMdf")]
        public async Task<IActionResult> AddMdf([FromBody] AddMdfCommandRequest request)
        {
            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Stok/AddBackPanel")]
        public async Task<IActionResult> AddBackPanel([FromBody] AddBackPanelCommandRequest request)
        {
            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Stok/AddScrap")]
        public async Task<IActionResult> AddScrap([FromBody] AddScrapCommandRequest request)
        {
            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("Update/Mdf/{MdfId}")]
        public async Task<IActionResult> UpdateMdf([FromBody] UpdateMdfCommandRequest request , [FromRoute] string MdfId)
        {
            request.OwnerId = OwnerId;
            request.MdfId = MdfId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        [HttpPut("Update/BackPanel/{BackPanelId}")]
        public async Task<IActionResult> UpdateBackPanel([FromBody] UpdateBackPanelCommandRequest request, [FromRoute] string BackPanelId)
        {
            request.OwnerId = OwnerId;

            request.BackPanelId = BackPanelId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("Update/Glue/{GlueId}")]
        public async Task<IActionResult> UpdateGlue([FromBody] UpdateGlueCommandRequest request, [FromRoute] string GlueId)
        {
            request.OwnerId = OwnerId;
            request.GlueId = GlueId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("Update/Scrap/{ScrapId}")]
        public async Task<IActionResult> UpdateScrap([FromBody] UpdateScrapCommandRequest request, [FromRoute] string ScrapId)
        {
            request.OwnerId = OwnerId;
            request.ScrapId = ScrapId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("Update/PvcBand/{PvcBandId}")]
        public async Task<IActionResult> UpdatePvcBand([FromBody] UpdatePvcBandCommandRequest request, [FromRoute] string PvcBandId)
        {
            request.OwnerId = OwnerId;
            request.PvcBandId = PvcBandId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
              
        [HttpPut("Stock/Reduce/Mdf/{MdfId}")]
        public async Task<IActionResult> ReduceMdf([FromBody] ReduceMdfCommandRequest request,[FromRoute] string MdfId)
        {
            request.MdfId = MdfId;
            request.OwnerID = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Stock/Reduce/PvcBand/{PvcBandId}")]
        public async Task<IActionResult> ReducePvcBand([FromBody] ReducePvcBandCommandRequest request,[FromRoute] string PvcBandId)
        {
            request.OwnerId = OwnerId;
            request.PvcBandId = PvcBandId;
            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Stock/Reduce/Glue/{GlueId}")]
        public async Task<IActionResult> ReduceGlue([FromBody] ReduceGlueCommandRequest request, [FromRoute] string GlueId)
        {
            request.OwnerID = OwnerId;
            request.GlueId = GlueId;
            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Stock/Reduce/Scraps/{ScrapsId}")]
        public async Task<IActionResult> ReduceScrap([FromBody] ReduceScrapCommandRequest request,[FromRoute] string ScrapsId)
        {
            request.ScrapsId = ScrapsId;
            request.OwnerId= OwnerId;
            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Stock/Reduce/BackPanel/{BackPanelId}")]
        public async Task<IActionResult> BackPanel([FromBody] ReduceBackPanelCommandRequest request, [FromRoute] string BackPanelId)
        {
            request.OwnerId = OwnerId;
            request.BackPanelId = BackPanelId;
            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
