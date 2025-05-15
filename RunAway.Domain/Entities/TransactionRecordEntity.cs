using System.ComponentModel.DataAnnotations;
using RunAway.Domain.Commons;
using RunAway.Domain.Enums;
using RunAway.Domain.ValueObjects;

namespace RunAway.Domain.Entities
{
    /// <summary>
    /// Represents a transaction record in the system.
    /// Required properties: TransactionStatus, Amount, RoomID, UserID, Guests
    /// </summary>
    public class TransactionRecordEntity : AuditableEntity<Guid>
    {
        private readonly List<Guest> _guests = [];

        #region Properties

        [Required]
        public TransactionStatus TransactionStatus { get; private set; }

        [Required]
        public Money Price { get; private set; }

        [Required]
        public Guid RoomID { get; private set; }

        [Required]
        public Guid UserID { get; private set; }

        [Required]
        public DateOnly CheckInDate { get; private set; }

        [Required]
        public DateOnly CheckOutDate { get; private set; }


        [Required]
        public IReadOnlyCollection<Guest> Guests => _guests.AsReadOnly();

        #endregion

        #region Navigation Properties

        public RoomEntity Room { get; private set; }

        public UserEntity User { get; private set; }

        #endregion

        private TransactionRecordEntity() { }

        public static TransactionRecordEntity Create(
            Guid id,
            Money price,
            Guid roomID,
            Guid userID,
            List<Guest> guests,
            DateOnly checkInDate,
            DateOnly checkOutDate)
        {
            // Validation 
            if (price == null)
                throw new ArgumentNullException(nameof(price), "Price cannot be null");
            if (price.Amount < 0)
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be lower than 0");
            if (guests.Count == 0)
                throw new ArgumentException("At least one guest is required", nameof(guests));
            if (checkInDate > checkOutDate)
                throw new ArgumentException("Checkout date cannot before checkin date", nameof(checkInDate));

            var entity = new TransactionRecordEntity
            {
                ID = id,
                TransactionStatus = TransactionStatus.Pending,
                Price = price,
                RoomID = roomID,
                UserID = userID,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate
            };

            // Add guests
            foreach (var guest in guests)
            {
                entity._guests.Add(guest);
            }

            return entity;
        }

        public void UpdateTransactionStatus(TransactionStatus transactionStatus)
        {
            if (transactionStatus == TransactionStatus)
            {
                throw new Exception("New transaction status is same as current transaction status");
            }

            TransactionStatus = transactionStatus;
        }
    }
}
