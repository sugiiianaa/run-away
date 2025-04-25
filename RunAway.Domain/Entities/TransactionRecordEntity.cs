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
        public Money Amount { get; private set; }

        [Required]
        public Guid RoomID { get; private set; }

        [Required]
        public Guid UserID { get; private set; }


        [Required]
        public IReadOnlyCollection<Guest> Guests => _guests.AsReadOnly();

        #endregion

        #region Navigation Properties

        public RoomEntity Room { get; private set; }

        public UserEntity User { get; private set; }

        #endregion

#pragma warning disable CS8618
        private TransactionRecordEntity() { }
#pragma warning restore CS8618

        public TransactionRecordEntity(
            Guid id,
            Money amount,
            Guid roomID,
            Guid userID,
            List<Guest> guests) : base(id)
        {
            ArgumentNullException.ThrowIfNull(amount, nameof(amount));
            ArgumentNullException.ThrowIfNull(guests, nameof(guests));

            if (!guests.Any())
            {
                throw new ArgumentException("At least one guest is required", nameof(guests));
            }

            Id = id;
            TransactionStatus = TransactionStatus.Pending;
            Amount = amount;
            RoomID = roomID;
            UserID = userID;
            _guests = guests.ToList();
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
