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
    public class TransactionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Create new transaction
        /// POST: /api/Transaction/create-transaction
        /// </summary>
        [HttpPost("create-transaction")]
        [Authorize(Policy = UserAuthorizationPolicy.RequireUserRole)]
        public async Task<ActionResult<ApiResponse<CreateTransactionResponseDto>>> CreateTransaction([FromBody] CreateTransactionRequestDto requestDTO)
        {
            // Get user email from JWT token
            var userEmail = UserClaimsHelper.GetUserEmail(User);
            if (string.IsNullOrEmpty(userEmail))
            {
                return this.ToApiError<CreateTransactionResponseDto>(401, "User not authenticated properly");
            }

            var command = new CreateTransactionCommand
            {
                RoomID = requestDTO.RoomID,
                UserEmail = userEmail,
                Guests = requestDTO.Guests,
                NumberOfRoom = requestDTO.NumberOfRoom,
                CheckInDate = requestDTO.CheckInDate,
                CheckOutDate = requestDTO.CheckOutDate
            };

            var result = await _mediator.Send(command);

            return result.ToApiResponse(201, "Transaction recorded successfully");
        }
    }
}
