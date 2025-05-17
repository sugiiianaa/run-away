using MediatR;
using RunAway.Application.Commons;
using RunAway.Application.Dtos.Transaction;
using RunAway.Application.IServices;
using RunAway.Domain.Enums;

namespace RunAway.Application.Features.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommand : IRequest<Result<UpdateTransactionResponseDto>>
    {
        public Guid TransactionID { get; set; }
        public int TransactionStatus { get; set; }
    }

    public class UpdateTransactionCommandHandler(
        ITransactionService transactionService) : IRequestHandler<UpdateTransactionCommand, Result<UpdateTransactionResponseDto>>
    {
        private readonly ITransactionService _transactionService = transactionService;

        public async Task<Result<UpdateTransactionResponseDto>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.GetTransactionByIDAsync(request.TransactionID);

            if (transaction == null)
            {
                return Result<UpdateTransactionResponseDto>.Failure("Transaction not found", 400, ErrorCode.NotFound);
            }

            if (Enum.IsDefined(typeof(TransactionStatus), request.TransactionStatus))
            {
                var status = (TransactionStatus)request.TransactionStatus;
                await _transactionService.UpdateTransactionStatusAsync(transaction, status);
            }
            else
            {
                return Result<UpdateTransactionResponseDto>.Failure("TransactionStatus not valid", 400, ErrorCode.InvalidArgument);
            }


            return Result<UpdateTransactionResponseDto>.Success(new UpdateTransactionResponseDto
            {
                TransactionID = transaction.ID,
                TransactionStatus = request.TransactionStatus,
            });
        }
    }
}
