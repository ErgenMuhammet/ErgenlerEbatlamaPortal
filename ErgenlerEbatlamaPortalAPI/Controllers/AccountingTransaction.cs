using Application.Features.Command.AccountingTransaction.AddExpense;
using Application.Features.Command.AccountingTransaction.AddIncome;
using Application.Features.Command.AccountingTransaction.AddInvoice;
using Application.Features.Command.AccountingTransaction.PayTheInvoice;
using Application.Features.Command.AccountingTransactionCommands.UpdateExpense;
using Application.Features.Command.OrderTransactionCommands.DeleteOrder;
using Application.Features.Query.AccountingTransactionQuery.GetAllExpense;
using Application.Features.Query.AccountingTransactionQuery.GetAllIncomes;
using Application.Features.Query.AccountingTransactionQuery.GetInvoices;
using Application.Features.Query.AccountingTransactionQuery.GetProfitLossSituation;
using Application.Features.Query.AccountingTransactionQuery.GetPayedInvoice;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ErgenlerEbatlamaPortalAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("portal/Accounting")]
    public class AccountingTransaction : ControllerBase
    {
        private readonly IMediator _mediatR;
        protected string OwnerId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public AccountingTransaction(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpGet("GetAllInvoice")]
        public async Task<IActionResult> GetInvoice()
        {
            var request = new GetAllInvoicesQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetPayedInvoice")]
        public async Task<IActionResult> GetPayedInvoice()
        {
            var request = new GetPayedInvoiceQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


        [HttpPost("AddInvoice")]
        public async Task<IActionResult> AddInvoice([FromBody] AddInvoiceCommandRequest request ) 
        {
            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("MarkThePayedToInvoice/{InvoiceId}")]
        public async Task<IActionResult> PayTheInvoice([FromRoute] string? InvoiceId)
        {            
            var request = new PayTheInvoiceCommanRequest();

            request.OwnerId = OwnerId;
            request.InvoiceId = InvoiceId;
            
            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("AddExpense")]
        public async Task<IActionResult> AddExpense([FromBody] AddExpenseCommandRequest request)
        {
            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("AddIncome")]
        public async Task<IActionResult> AddIncome([FromBody] AddIncomeCommandRequest request)
        {
            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetAllIncome")]
        public async Task<IActionResult> GetAllIncome()
        {
            var request = new GetAllIncomesQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetAllExpense")]
        public async Task<IActionResult> GetExpense()
        {
            var request = new GetAllExpenseQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch("UpdateExpense/{ExpenseId}")]
        public async Task<IActionResult> UpdateExpense([FromBody] UpdateExpenseCommandRequest request, [FromRoute] string? ExpenseId)
        {
            request.OwnerId = OwnerId;
            request.ExpenseId = ExpenseId;
                
            var result = await _mediatR.Send(request);

            if (!result.IsSucces)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetProfitLossSituation")]
        public async Task<IActionResult> GetProfitLossSituation()
        {
            var request = new GetProfitLossSituationQueryRequest();

            request.OwnerId = OwnerId;

            var result = await _mediatR.Send(request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
