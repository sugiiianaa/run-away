namespace RunAway.Application.Dtos.Transaction
{
    // Request
    public class UpdateTransactionRequestDto
    {
        public Guid TransactionID { get; set; }
        public int TransactionStatus { get; set; }
    }

    // Response
    public class UpdateTransactionResponseDto
    {
        public Guid TransactionID { get; set; }
        public int TransactionStatus { get; set; }
    }
}
