using MediatR;
using Microsoft.Extensions.Logging;
using RunAway.Application.Commons;
using RunAway.Application.Dtos.User;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Result<CreateUserResponseDto>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserResponseDto>>
    {
        private readonly IUserService _userService;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IUserService userService,
            ILogger<CreateUserCommandHandler> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        public async Task<Result<CreateUserResponseDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user already exist
            var existingUser = await _userService.GetUserEntityByEmailAsync(request.Email);

            if (existingUser != null)
            {
                Result<CreateUserResponseDto>.Failure("User already registered", 400, ErrorCode.InvalidOperation);
            }
            var newUser = await _userService.CreateUserAsync(request, cancellationToken);

            return Result<CreateUserResponseDto>.Success(new CreateUserResponseDto
            {
                Id = newUser.ID,
                Email = newUser.Email
            });
        }
    }

}
