using MediatR;
using RunAway.Application.Commons;
using RunAway.Application.Dtos.User;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Users.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<Result<LoginUserResponseDto>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginUserQueryHandler(
        IUserService userService,
        IPasswordService passwordService,
        IAuthService authService) : IRequestHandler<LoginUserQuery, Result<LoginUserResponseDto>>
    {
        private readonly IUserService _userService = userService;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IAuthService _authService = authService;

        public async Task<Result<LoginUserResponseDto>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            // Check if user is exist
            var user = await _userService.GetUserEntityByEmailAsync(request.Email);

            if (user == null)
            {
                return Result<LoginUserResponseDto>.Failure("User is not registered", 400, ErrorCode.InvalidArgument);
            }

            if (!_passwordService.VerifyPassword(request.Password, user.Password))
            {
                return Result<LoginUserResponseDto>.Failure("Username or password is invalid", 400, ErrorCode.InvalidOperation);
            }

            var (token, expiresAt) = _authService.GenerateToken(user.ID, user.Email, user.Role);

            return Result<LoginUserResponseDto>.Success(new LoginUserResponseDto
            {
                Token = token,
                ExpiredDate = expiresAt
            });
        }
    }
}
