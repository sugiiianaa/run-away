using MediatR;
using RunAway.Application.Dtos;
using RunAway.Application.IRepositories;
using RunAway.Application.IServices;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Features.Users.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<TokenDto>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, TokenDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IAuthService _authService;

        public LoginUserQueryHandler(IUserRepository userRepository, IPasswordService passwordService, IAuthService authService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _authService = authService;
        }

        public async Task<TokenDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(new Email(request.Email));

            if (user == null)
            {
                throw new NullReferenceException(nameof(user));
            }

            if (!_passwordService.VerifyPassword(request.Password, user.Password))
            {
                throw new ArgumentException("Email or password are invalid");
            }

            var tokenData = _authService.GenerateToken(user.Id, user.Email);

            return AuthMapper.ToTokenDto(tokenData.token, tokenData.expiresAt);

        }
    }
}
