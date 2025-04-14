using MediatR;
using Microsoft.Extensions.Logging;
using RunAway.Application.IRepositories;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public Email Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateUserCommandHandler> _logger;


        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.NewGuid();

            var email = new Email(request.Email);

            var user = new UserEntity(
                userId,
                email,
                request.Password,
                request.Name);

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User {UserId} created successfully", user.Id);

            return user.Id;
        }
    }

}
