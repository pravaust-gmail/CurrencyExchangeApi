using CurrencyExchangeApi.Common;
using Microsoft.AspNetCore.Mvc;
using CurrencyExchangeApi.Models.Responses;
using CurrencyExchangeApi.Models.Requests;
using CurrencyExchangeApi.Services;
using CurrencyExchangeApi.Validators;

namespace CurrencyExchangeApi.Controllers
{
    [ApiController]
    [Route("transfers")]
    public class TransfersController(IQuoteService quoteService, IQuoteRequestValidator validator, ITransferService transferService)
        : ControllerBase
    {
        // POST /transfers/quote
        [HttpPost("quote")]
        public async Task<ActionResult<CreateQuoteResponse>> CreateQuote([FromBody] CreateQuoteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var validationErrors = validator.Validate(request);
            if (validationErrors.Any())
                return BadRequest(new { Errors = validationErrors });
  
            
            var result = await quoteService.CreateQuote(request);

            return HandleResult(result);
        }
        
        // GET /transfers/quote/{quoteId}
        [HttpGet("quote/{quoteId}")]
        public ActionResult<CreateQuoteResponse> GetQuote(Guid quoteId)
        {
            var result = quoteService.GetQuoteById(quoteId);
            return HandleResult(result);
        }
        
        // POST /transfers
        [HttpPost]
        public ActionResult<CreateTransferResponse> CreateTransfer([FromBody] CreateTransferRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = transferService.CreateTransfer(request);
            return HandleResult(result);
        }

        // GET /transfers/{transferId}
        [HttpGet("{transferId}")]
        public ActionResult<CreateTransferResponse> GetTransfer(Guid transferId)
        {
            var result = transferService.GetTransferById(transferId);
            return HandleResult(result);
        }
        
        private ActionResult<T> HandleResult<T>(Result<T> result)
        {
            if (!result.Success)
                return NotFound(new { result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
