namespace RunAway.Application.IServices
{
    public interface IPasswordService
    {
        string HashPassword(string password, int workFactor = 12);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
