namespace RunAway.Application.IServices
{
    public interface IAuthService
    {
        public (string token, DateTime expiresAt) GenerateToken(Guid ID, string Email);
    }
}
