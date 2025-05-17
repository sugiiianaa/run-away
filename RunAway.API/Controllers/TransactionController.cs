using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RunAway.API.Helpers;
using RunAway.Application.Dtos.Transaction;
using RunAway.Application.Features.Transactions.Commands.CreateTransaction;
using RunAway.Application.Features.Transactions.Commands.UpdateTransaction;
using RunAway.Application.Features.Transactions.Queries.GetTransaction;
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

        /// <summary>
        /// Get user transaction
        /// GET: /api/Transaction/get-transaction?PageNumber={int}&BatchSize={int}
        /// </summary>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        [HttpGet("get-transaction")]
        [Authorize(Policy = UserAuthorizationPolicy.RequireUserRole)]
        public async Task<ActionResult<ApiResponse<GetTransactionResponseDto>>> GetTransaction([FromQuery] GetTransactionRequestDto requestDTO)
        {
            // Get user email from JWT token
            var userID = UserClaimsHelper.GetUserId(User);

            if (userID == Guid.Empty || userID == null)
            {
                return this.ToApiError<GetTransactionResponseDto>(401, "User not authenticated properly");
            }

            var query = new GetTransactionQuery
            {
                BatchSize = requestDTO.BatchSize,
                PageNumber = requestDTO.PageNumber,
                TransactionStatus = requestDTO.TransactionStatus,
                UserID = userID.Value
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return this.ToApiError<GetTransactionResponseDto>(result.ApiResponseErrorCode, result.ErrorMessage);
            }

            return result.ToApiResponse<GetTransactionResponseDto>(200);
        }


        /// <summary>
        /// Update user transaction manually (admin & super user only)
        /// PUT: /api/Transaction/update-transaction
        /// </summary>
        /// <returns></returns>
        [HttpPut("update-transaction")]
        [Authorize(Policy = UserAuthorizationPolicy.RequireAdminRole)]
        public async Task<ActionResult<ApiResponse<UpdateTransactionResponseDto>>> UpdateTransaction([FromBody] UpdateTransactionRequestDto requestDTO)
        {
            var command = new UpdateTransactionCommand
            {
                TransactionID = requestDTO.TransactionID,
                TransactionStatus = requestDTO.TransactionStatus
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return this.ToApiError<UpdateTransactionResponseDto>(result.ApiResponseErrorCode, result.ErrorMessage);
            }

            return result.ToApiResponse<UpdateTransactionResponseDto>(201);
        }
    }
}
