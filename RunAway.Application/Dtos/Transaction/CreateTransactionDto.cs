using RunAway.Domain.Enums;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Dtos.Transaction
{
    // Request
    public class CreateTransactionRequestDto
    {
        public required Guid RoomID { get; set; }
        public required IList<Guest> Guests { get; set; }
        public required int NumberOfRoom { get; set; }
        public required DateOnly CheckInDate { get; set; }
        public required DateOnly CheckOutDate { get; set; }
    }

    // Response
    public class CreateTransactionResponseDto
    {
        public Guid TransactionID { get; set; }
        public required Guid RoomId { get; set; }
        public required decimal UnitPrice { get; set; }
        public required int Quantity { get; set; }
        public required int NumberOfDays { get; set; }
        public required decimal Discount { get; set; } = 0;
        public required decimal Fee { get; set; } = 0;
        public required decimal TotalPrice { get; set; }
        public required TransactionStatus Status { get; set; }
    }

}
