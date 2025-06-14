using System.Diagnostics.CodeAnalysis;

namespace EquipmentRepairDocument.Core.Data.Users
{
    public class User : Entity
    {
        [SetsRequiredMembers]
        public User(string username, string passwordHash, UserRole userRole)
            : this(username, passwordHash) => UserRole = userRole;

        [SetsRequiredMembers]
        public User(string username, string passwordHash)
        {
            ArgumentException.ThrowIfNullOrEmpty(username);
            ArgumentException.ThrowIfNullOrEmpty(passwordHash);

            Username = username;
            PasswordHash = passwordHash;
        }

        private User()
        {
        }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Хеш пароля.
        /// </summary>
        public required string PasswordHash { get; set; }

        /// <summary>
        /// Роль пользователя в прилажении.
        /// </summary>
        public UserRole UserRole { get; set; } = UserRole.Viewer;

        public bool Equals(User? user)
            => user is not null && user.Username == Username && user.PasswordHash == PasswordHash;
    }
}
