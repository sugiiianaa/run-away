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
        public PriceBreakdown PriceBreakdown { get; private set; }

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
            string currency,
            decimal unitPrice,
            int numberOfUnits,
            int numberOfDays,
            decimal discount,
            decimal fee,
            Guid roomID,
            Guid userID,
            List<Guest> guests,
            DateOnly checkInDate,
            DateOnly checkOutDate)
        {
            // Create price breakdown
            var priceBreakdown = PriceBreakdown.Create(
                currency,
                unitPrice,
                numberOfUnits,
                numberOfDays,
                discount,
                fee);

            // Validation 
            if (priceBreakdown.TotalPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(priceBreakdown), "Total price cannot be lower than 0");
            if (guests.Count == 0)
                throw new ArgumentException("At least one guest is required", nameof(guests));
            if (checkInDate > checkOutDate)
                throw new ArgumentException("Checkout date cannot before checkin date", nameof(checkInDate));

            var entity = new TransactionRecordEntity
            {
                ID = id,
                TransactionStatus = TransactionStatus.Pending,
                PriceBreakdown = priceBreakdown,
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

        // Price breakdown update methods
        public void UpdateUnitPrice(decimal newUnitPrice)
        {
            PriceBreakdown.UpdateUnitPrice(newUnitPrice);
        }


        public void UpdateDiscount(decimal newDiscount)
        {
            PriceBreakdown.UpdateDiscount(newDiscount);
        }

        public void UpdateFee(decimal newFee)
        {
            PriceBreakdown.UpdateFee(newFee);
        }

        // Convenience getter for total price as Money
        public Money GetTotalPrice()
        {
            return PriceBreakdown.GetTotalAsMoney();
        }
    }
}
