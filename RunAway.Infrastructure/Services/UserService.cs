using RunAway.Application.Dtos.User;
using RunAway.Application.Features.Users.Commands.CreateUser;
using RunAway.Application.IRepositories;
using RunAway.Application.IServices;
using RunAway.Domain.Commons;
using RunAway.Domain.Entities;
using RunAway.Domain.ValueObjects;

namespace RunAway.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserEntity> CreateUserAsync(CreateUserCommand command, CancellationToken cancellationToken)
        {
            command.Password = _passwordService.HashPassword(command.Password);
            var userEntity = CreateUserMapper.ToUserEntity(command);

            await _userRepository.AddAsync(userEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return userEntity;
        }

        public async Task<UserEntity?> GetUserEntityByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(new Email(email));
            return user;
        }
    }
}
