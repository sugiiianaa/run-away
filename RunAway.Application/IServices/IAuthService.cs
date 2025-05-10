using RunAway.Domain.Enums;

namespace RunAway.Application.IServices
{
    public interface IAuthService
    {
        public (string token, DateTime expiresAt) GenerateToken(Guid ID, string Email, UserRoles role);
    }
}
