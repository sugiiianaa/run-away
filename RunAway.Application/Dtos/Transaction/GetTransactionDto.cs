using RunAway.Domain.Enums;

namespace RunAway.Application.Dtos.Transaction
{
    // Request
    public class GetTransactionRequestDto
    {
        public required int PageNumber { get; set; } = 1;
        public required int BatchSize { get; set; } = 30;
        public int TransactionStatus { get; set; } = -1;
    }
    // Response
    public class GetTransactionResponseDto
    {
        public required int TransactionCount { get; set; }
        public required IList<GetTransacationDto> Transactions { get; set; }
    }

    public class GetTransacationDto
    {
        public required Guid TransactionID { get; set; }
        public required Guid RoomID { get; set; }
        public required int RoomQuantity { get; set; }
        public required int NumberOfDays { get; set; }
        public required decimal Discount { get; set; }
        public required decimal Fee { get; set; }
        public required decimal TotalPrice { get; set; }
        public required TransactionStatus TransactionStatus { get; set; }
    }

}
