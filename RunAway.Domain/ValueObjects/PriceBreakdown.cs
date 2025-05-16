namespace RunAway.Domain.ValueObjects
{
    /// <summary>
    /// Represents a complete price breakdown for a transaction
    /// </summary>
    public class PriceBreakdown
    {
        // Base currency for the entire price breakdown
        public string Currency { get; private set; }

        // Unit price per night/item
        public decimal UnitPrice { get; private set; }

        // Number of units (e.g., 1 rooms)
        public int Quantity { get; private set; }

        // Number of days (e.g., 1 nights)
        public int NumberOfDays { get; private set; }

        // Discount amount (if any)
        public decimal Discount { get; private set; }

        // Additional fee amount (if any)
        public decimal Fee { get; private set; }

        // Total amount after calculation
        public decimal TotalPrice { get; private set; }

        private PriceBreakdown() { }

        public static PriceBreakdown Create(
            string currency,
            decimal unitPrice,
            int quantity,
            int numberOfDays,
            decimal discount = 0,
            decimal fee = 0)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be null or empty", nameof(currency));

            if (unitPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative");

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero");

            if (numberOfDays <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfDays), "Number of days must be greater than zero");

            if (discount < 0)
                throw new ArgumentOutOfRangeException(nameof(discount), "Discount cannot be negative");

            if (fee < 0)
                throw new ArgumentOutOfRangeException(nameof(fee), "Fee cannot be negative");

            // Calculate the total amount
            decimal subtotal = unitPrice * quantity * numberOfDays;
            decimal totalPrice = subtotal - discount + fee;

            // Ensure total isn't negative after discount
            if (totalPrice < 0)
                totalPrice = 0;

            return new PriceBreakdown
            {
                Currency = currency,
                UnitPrice = unitPrice,
                Quantity = quantity,
                NumberOfDays = numberOfDays,
                Discount = discount,
                Fee = fee,
                TotalPrice = totalPrice
            };
        }

        // Create money objects when needed
        public Money GetUnitPriceAsMoney() => new Money(UnitPrice, Currency);
        public Money GetDiscountAsMoney() => new Money(Discount, Currency);
        public Money GetFeeAsMoney() => new Money(Fee, Currency);
        public Money GetTotalAsMoney() => new Money(TotalPrice, Currency);

        // Update methods with recalculation
        public void UpdateUnitPrice(decimal newUnitPrice)
        {
            if (newUnitPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(newUnitPrice), "Unit price cannot be negative");

            UnitPrice = newUnitPrice;
            RecalculateTotal();
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(newQuantity), "Quantity must be greater than zero");

            Quantity = newQuantity;
            RecalculateTotal();
        }

        public void UpdateNumberOfDays(int newNumberOfDays)
        {
            if (newNumberOfDays <= 0)
                throw new ArgumentOutOfRangeException(nameof(newNumberOfDays), "Number of days must be greater than zero");

            NumberOfDays = newNumberOfDays;
            RecalculateTotal();
        }

        public void UpdateDiscount(decimal newDiscount)
        {
            if (newDiscount < 0)
                throw new ArgumentOutOfRangeException(nameof(newDiscount), "Discount cannot be negative");

            Discount = newDiscount;
            RecalculateTotal();
        }

        public void UpdateFee(decimal newFee)
        {
            if (newFee < 0)
                throw new ArgumentOutOfRangeException(nameof(newFee), "Fee cannot be negative");

            Fee = newFee;
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            decimal subtotal = UnitPrice * Quantity * NumberOfDays;
            TotalPrice = subtotal - Discount + Fee;

            // Ensure total isn't negative after discount
            if (TotalPrice < 0)
                TotalPrice = 0;
        }
    }
}
