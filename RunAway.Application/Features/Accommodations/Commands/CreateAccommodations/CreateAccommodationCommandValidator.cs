using FluentValidation;
using RunAway.Application.Dtos.Room;

namespace RunAway.Application.Features.Accommodations.Commands.CreateAccommodations
{
    public class CreateAccommodationCommandValidator : AbstractValidator<CreateAccommodationCommand>
    {
        public CreateAccommodationCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(c => c.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

            RuleFor(c => c.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(c => c.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");

            RuleFor(c => c.ImageUrls)
                .NotEmpty().WithMessage("At least one image URL is required.")
                .Must(list => list.All(url => Uri.TryCreate(url, UriKind.Absolute, out _)))
                .WithMessage("All image URLs must be valid URLs.");

            RuleFor(c => c.Rooms)
                .NotEmpty().WithMessage("At least one room is required.");

            RuleForEach(c => c.Rooms).SetValidator(new CreateRoomDtoValidator());
        }
    }

    public class CreateRoomDtoValidator : AbstractValidator<CreateRoomRequestDto>
    {
        public CreateRoomDtoValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Room name is required.")
                .MaximumLength(100).WithMessage("Room name must not exceed 100 characters.");

            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("Room description is required.")
                .MaximumLength(1000).WithMessage("Room description must not exceed 1000 characters.");

            RuleFor(r => r.Price.Amount)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(r => r.Price.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Must(c => IsValidCurrency(c)).WithMessage("Currency must be a valid ISO currency code.");

            RuleFor(r => r.Facilities)
                .NotEmpty().WithMessage("At least one facility is required.");
        }

        private bool IsValidCurrency(string currencyCode)
        {
            var validCodes = new[] { "USD", "EUR", "GBP", "IDR", "JPY" };
            return validCodes.Contains(currencyCode);
        }
    }
}
