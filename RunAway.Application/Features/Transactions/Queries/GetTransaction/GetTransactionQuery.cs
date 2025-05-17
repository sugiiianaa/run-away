using MediatR;
using RunAway.Application.Commons;
using RunAway.Application.Dtos.Transaction;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Transactions.Queries.GetTransaction
{
    public class GetTransactionQuery : IRequest<Result<GetTransactionResponseDto>>
    {
        public required Guid UserID { get; set; }
        public required int BatchSize { get; set; } = 30;
        public required int PageNumber { get; set; } = 1;
        public required int TransactionStatus { get; set; }
    }

    public class GetTransactionCommandHandler(
        ITransactionService transactionService) : IRequestHandler<GetTransactionQuery, Result<GetTransactionResponseDto>>
    {
        private readonly ITransactionService _transactionService = transactionService;

        public async Task<Result<GetTransactionResponseDto>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            // Validation query
            if (request.BatchSize > 30 || request.BatchSize < 1)
            {
                return Result<GetTransactionResponseDto>.Failure("Batch size should lower than 30 and higher than 0", 400, ErrorCode.InvalidArgument);
            }

            if (request.PageNumber < 1)
            {
                return Result<GetTransactionResponseDto>.Failure("Page number should greater than 0", 400, ErrorCode.InvalidArgument);
            }

            if (request.UserID == Guid.Empty)
            {
                return Result<GetTransactionResponseDto>.Failure("Invalid authorization process", 401, ErrorCode.Unauthorized);
            }

            var transactionCount = -1;

            if (request.PageNumber == 1)
            {
                transactionCount = await _transactionService.GetTransactionCountAsync(request.UserID, request.TransactionStatus);

                if (transactionCount < 1)
                {
                    return Result<GetTransactionResponseDto>.Failure("No transaction record found", 404, ErrorCode.NotFound);
                }
            }

            var transactions = await _transactionService.GetTransactionAsync(request.UserID, request.TransactionStatus, request.PageNumber, request.BatchSize);

            return Result<GetTransactionResponseDto>.Success(new GetTransactionResponseDto
            {
                TransactionCount = transactionCount,
                Transactions = transactions.Select(t => new GetTransacationDto
                {
                    TransactionID = t.ID,
                    RoomID = t.RoomID,
                    RoomQuantity = t.PriceBreakdown.Quantity,
                    NumberOfDays = t.PriceBreakdown.NumberOfDays,
                    Discount = t.PriceBreakdown.Discount,
                    Fee = t.PriceBreakdown.Fee,
                    TotalPrice = t.PriceBreakdown.TotalPrice,
                    TransactionStatus = t.TransactionStatus
                }).ToList()
            });
        }
    }
}