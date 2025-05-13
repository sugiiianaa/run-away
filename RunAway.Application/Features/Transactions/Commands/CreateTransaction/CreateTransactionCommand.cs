using MediatR;
using RunAway.Application.Dtos.Transaction;
using RunAway.Application.IServices;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<CreateTransactionResponseDto?>
    {
        public required Guid RoomId { get; set; }
        public required Guid UserId { get; set; }
        public required IList<Guest> Guests { get; set; }
    };

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, CreateTransactionResponseDto?>
    {
        private readonly ITransactionService _transactionService;
        private readonly IRoomService _roomService;

        public CreateTransactionCommandHandler(ITransactionService transactionService, IRoomService roomService)
        {
            _transactionService = transactionService;
            _roomService = roomService;
        }

        public async Task<CreateTransactionResponseDto?> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            // TODO : should check if user exist 

            var room = await _roomService.GetRoomByIDAsync(request.RoomId);

            if (room == null)
                // TODO: should return proper error
                return null;

            // TODO: should add check availability method later

            var transaction = await _transactionService.CreateTransactionAsync(request.RoomId, request.UserId, room.Price, request.Guests, cancellationToken);

            return new CreateTransactionResponseDto
            {
                TransactionID = transaction.ID,
                RoomId = transaction.RoomID,
                Price = transaction.Price,
                Status = transaction.TransactionStatus,
            };
        }
    }
}
