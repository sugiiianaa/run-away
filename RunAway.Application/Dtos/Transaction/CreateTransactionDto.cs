using RunAway.Domain.Enums;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Dtos.Transaction
{
    // Request

    // Response
    public class CreateTransactionResponseDto
    {
        public Guid TransactionID { get; set; }
        public required Guid RoomId { get; set; }
        public required Money Price { get; set; }
        public required TransactionStatus Status { get; set; }
    }

}
