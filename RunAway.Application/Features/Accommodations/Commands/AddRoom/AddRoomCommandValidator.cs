using FluentValidation;

namespace RunAway.Application.Features.Accommodations.Commands.AddRoom
{
    public class AddRoomCommandValidator : AbstractValidator<AddRoomCommand>
    {
        public AddRoomCommandValidator()
        {
            RuleFor(c => c.AccommodationId)
              .NotEmpty().WithMessage("Accommodation ID is required.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Room name is required.")
                .MaximumLength(100).WithMessage("Room name must not exceed 100 characters.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("Room description is required.")
                .MaximumLength(1000).WithMessage("Room description must not exceed 1000 characters.");

            RuleFor(c => c.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(c => c.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Must(c => IsValidCurrency(c)).WithMessage("Currency must be a valid ISO currency code.");

            RuleFor(c => c.Facilities)
                .NotEmpty().WithMessage("At least one facility is required.");
        }

        private bool IsValidCurrency(string currencyCode)
        {
            var validCodes = new[] { "USD", "EUR", "GBP", "IDR", "JPY" };
            return validCodes.Contains(currencyCode);
        }
    }
}
