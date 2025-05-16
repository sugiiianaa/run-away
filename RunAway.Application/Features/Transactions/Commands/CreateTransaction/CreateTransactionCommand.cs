using MediatR;
using RunAway.Application.Commons;
using RunAway.Application.Dtos.Transaction;
using RunAway.Application.IServices;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<Result<CreateTransactionResponseDto>>
    {
        public required Guid RoomID { get; set; }
        public required string UserEmail { get; set; }
        public required IList<Guest> Guests { get; set; }
        public required int NumberOfRoom { get; set; }
        public required DateOnly CheckInDate { get; set; }
        public required DateOnly CheckOutDate { get; set; }
    };

    public class CreateTransactionCommandHandler(
        ITransactionService transactionService,
        IRoomService roomService,
        IAccommodationAvailabilityService accommodationAvailabilityService,
        IUserService userService) : IRequestHandler<CreateTransactionCommand, Result<CreateTransactionResponseDto>>
    {
        private readonly ITransactionService _transactionService = transactionService;
        private readonly IRoomService _roomService = roomService;
        private readonly IAccommodationAvailabilityService _accommodationAvailabilityService = accommodationAvailabilityService;
        private readonly IUserService _userService = userService;

        public async Task<Result<CreateTransactionResponseDto>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {

            // Validate date range
            if (request.CheckInDate >= request.CheckOutDate)
            {
                return Result<CreateTransactionResponseDto>.Failure(
                    "Check-in date must be before check-out date",
                    400,
                    ErrorCode.InvalidArgument);
            }

            // Find user by email
            var user = await _userService.GetUserEntityByEmailAsync(request.UserEmail);
            if (user == null)
            {
                return Result<CreateTransactionResponseDto>.Failure(
                    "User not found",
                    401,
                    ErrorCode.Unauthorized);
            }

            // Check if room exists
            var room = await _roomService.GetRoomByIDAsync(request.RoomID);
            if (room == null)
            {
                return Result<CreateTransactionResponseDto>.Failure(
                    "Room not found",
                    400,
                    ErrorCode.InvalidArgument);
            }

            // Check room availability for the date range
            var roomAvailabilities = await _accommodationAvailabilityService.GetRoomAvailabilityOnDateRangeAsync(
                request.RoomID,
                request.CheckInDate,
                request.CheckOutDate,
                request.NumberOfRoom);

            if (roomAvailabilities == null || !roomAvailabilities.Any())
            {
                return Result<CreateTransactionResponseDto>.Failure(
                    "No availability records found for the specified date range",
                    400,
                    ErrorCode.InvalidArgument);
            }

            // Calculate the number of days in the range
            int daysInRange = (request.CheckOutDate.DayNumber - request.CheckInDate.DayNumber);

            // Ensure we have availability records for every day in the range
            if (roomAvailabilities.Count < daysInRange)
            {
                return Result<CreateTransactionResponseDto>.Failure(
                    "Room is not available for the entire date range",
                    400,
                    ErrorCode.InvalidArgument);
            }

            // All validations passed, create the transaction
            var transaction = await _transactionService.CreateTransactionAsync(
                request.RoomID,
                user.ID,
                "USD", // TODO : should make it dynamic later
                room.Price.Amount,
                request.NumberOfRoom,
                daysInRange,
                0,// TODO : should make it dynamic later
                0,// TODO : should make it dynamic later
                request.Guests,
                request.CheckInDate,
                request.CheckOutDate,
                cancellationToken);

            // Update room availability to decrease available rooms
            var updateResult = await _accommodationAvailabilityService.UpdateRoomAvailabilityAsync(
                request.RoomID,
                request.CheckInDate,
                request.CheckOutDate,
                request.NumberOfRoom);

            if (!updateResult.IsSuccess)
            {
                // If updating availability fails, return the error
                return Result<CreateTransactionResponseDto>.Failure(
                    updateResult.ErrorMessage,
                    updateResult.ApiResponseErrorCode,
                    updateResult.ErrorCode);
            }

            var responseDto = new CreateTransactionResponseDto
            {
                TransactionID = transaction.ID,
                RoomId = transaction.RoomID,
                UnitPrice = transaction.PriceBreakdown.UnitPrice,
                Quantity = transaction.PriceBreakdown.Quantity,
                NumberOfDays = transaction.PriceBreakdown.NumberOfDays,
                Discount = transaction.PriceBreakdown.Discount,
                Fee = transaction.PriceBreakdown.Fee,
                TotalPrice = transaction.PriceBreakdown.TotalPrice,
                Status = transaction.TransactionStatus,
            };

            return Result<CreateTransactionResponseDto>.Success(responseDto);
        }
    }
}
