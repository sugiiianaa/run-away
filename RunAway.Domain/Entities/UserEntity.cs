using RunAway.Domain.Commons;
using RunAway.Domain.Enums;
using RunAway.Domain.ValueObjects;

namespace RunAway.Domain.Entities
{
    /// <summary>
    /// Represents an user in the system.
    /// Required propertiesL: Email, Password, Name
    /// </summary>
    public class UserEntity : AuditableEntity<Guid>
    {
        #region Properties

        public Email Email { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public UserRoles Role { get; private set; }

        #endregion

        #region Collection Properties

        private readonly List<TransactionRecordEntity> _transactions = [];
        public IReadOnlyCollection<TransactionRecordEntity> Transactions => _transactions.AsReadOnly();

        #endregion

        private UserEntity() { } // For entity framework

        public static UserEntity Create(
            Guid id,
            Email email,
            string password,
            string name,
            UserRoles role)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password cannot be empty.", nameof(password));

            if (password.Length < 8)
                throw new ArgumentException("Password must contains at least 8 character");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Name cannot be empty.", nameof(name));

            if (email == null)
                throw new ArgumentNullException(nameof(email), "Email is required.");

            var entity = new UserEntity
            {
                Id = id,
                Email = email,
                Password = password,
                Name = name,
                Role = role
            };

            return entity;
        }

        public void UpdateRoles(UserRoles role)
        {
            Role = role;
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
