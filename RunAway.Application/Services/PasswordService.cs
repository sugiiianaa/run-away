using RunAway.Application.IServices;

namespace RunAway.Application.Services
{
    public class PasswordService : IPasswordService
    {
        // Higher defaultWorkFactor -> more secure -> slower
        private const int DefaultWorkFactor = 12;

        public string HashPassword(string password, int workFactor = DefaultWorkFactor)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");

            // Generate a cryptographically secure salt and hash the password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor);

            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");

            if (string.IsNullOrEmpty(hashedPassword))
                throw new ArgumentNullException(nameof(hashedPassword), "Hashed password cannot be null or empty");

            try
            {
                // BCrypt.Verify compares the password with the hash in a time-constant manner
                // which helps prevent timing attacks
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch (FormatException)
            {
                // If the hash format is invalid, return false rather than throwing an exception
                return false;
            }
        }
    }
}
