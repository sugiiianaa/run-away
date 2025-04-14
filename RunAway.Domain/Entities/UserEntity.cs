using RunAway.Domain.Commons;
using RunAway.Domain.ValueObjects;

namespace RunAway.Domain.Entities
{
    public class UserEntity : AuditableEntity<Guid>
    {
        public Email Email { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }

        private UserEntity() { } // For entity framework

        public UserEntity(
            Guid id,
            Email email,
            string password,
            string name) : base(id)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password cannot be empty.", nameof(password));

            if (password.Length < 8)
                throw new ArgumentException("Password must contains at least 8 character");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Name cannot be empty.", nameof(name));

            if (email == null)
                throw new ArgumentNullException(nameof(email), "Email is required.");

            Id = id;
            Email = email;
            Password = password;
            Name = name;
        }

        public void UpdatePassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentNullException("Password cannot be empty.", nameof(newPassword));

            if (newPassword.Length < 8)
                throw new ArgumentException("Password must contains at least 8 character");

            Password = newPassword;
        }
    }
}
