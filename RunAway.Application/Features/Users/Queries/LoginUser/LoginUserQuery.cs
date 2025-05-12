using MediatR;
using RunAway.Application.Dtos.User;
using RunAway.Application.IServices;

namespace RunAway.Application.Features.Users.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<LoginUserResponseDto?>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, LoginUserResponseDto?>
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IAuthService _authService;

        public LoginUserQueryHandler(
            IUserService userService,
            IPasswordService passwordService,
            IAuthService authService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _authService = authService;
        }

        public async Task<LoginUserResponseDto?> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            // Check if user is exist
            var user = await _userService.GetUserEntityByEmailAsync(request.Email);

            if (user == null)
                // should return more proper value to make it easier found out what the problem later.
                return null;

            if (!_passwordService.VerifyPassword(request.Password, user.Password))
                // should return more proper value to make it easier found out what the problem later.
                return null;


            var (token, expiresAt) = _authService.GenerateToken(user.Id, user.Email, user.Role);

            return new LoginUserResponseDto
            {
                Token = token,
                ExpiredDate = expiresAt
            };
        }
    }
}
