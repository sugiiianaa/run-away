using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunAway.API.Helpers;
using RunAway.Application.Dtos.Transaction;
using RunAway.Application.Features.Transactions.Commands.CreateTransaction;
using RunAway.Infrastructure.Constants;

namespace RunAway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create new transaction
        /// </summary>
        /// <returns></returns>
        [HttpPost("create-transaction")]
        [Authorize(Policy = UserAuthorizationPolicy.RequireUserRole)]
        public async Task<ActionResult<ApiResponse<CreateTransactionResponseDto>>> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var result = await _mediator.Send(command);

            if (result == null)
                return this.ToApiError<CreateTransactionResponseDto>(400, "Invalid input");

            return result.ToApiResponse(201, "Transaction recorded successfully");
        }
    }
}
