using MediatR;
using Microsoft.Extensions.Logging;
using RunAway.Application.Dtos.User;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<CreateUserResponseDto?>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponseDto?>
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


        public async Task<CreateUserResponseDto?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user already exist
            var existingUser = await _userService.GetUserEntityByEmailAsync(request.Email);

            if (existingUser != null)
                // should return more proper value to make it easier found out what the problem later.
                return null;

            var newUser = await _userService.CreateUserAsync(request, cancellationToken);

            return new CreateUserResponseDto
            {
                Id = newUser.ID,
                Email = newUser.Email
            };
        }
    }

}
