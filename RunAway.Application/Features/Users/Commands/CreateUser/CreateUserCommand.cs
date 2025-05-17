using MediatR;
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

    public class CreateUserCommandHandler(
        IUserService userService) : IRequestHandler<CreateUserCommand, Result<CreateUserResponseDto>>
    {
        private readonly IUserService _userService = userService;

        public async Task<Result<CreateUserResponseDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userService.GetUserEntityByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return Result<CreateUserResponseDto>.Failure("User already registered", 400, ErrorCode.InvalidOperation);
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
