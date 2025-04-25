using MediatR;
using Microsoft.Extensions.Logging;
using RunAway.Application.Dtos;
using RunAway.Application.IRepositories;
using RunAway.Application.IServices;
using RunAway.Domain.Commons;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserIdDto>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserIdDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IPasswordService _passwordService;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateUserCommandHandler> logger,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _passwordService = passwordService;
        }

        public async Task<UserIdDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var email = new Email(request.Email);
            var hashedPassword = _passwordService.HashPassword(request.Password);
            var user = UserMapper.ToUserEntity(Guid.NewGuid(), email, hashedPassword, request.Name);

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} created successfully", user.Id);

            var result = UserMapper.ToUserIdDto(user);

            return result;
        }
    }

}
