using EquipmentRepairDocument.Core.Data.Users;
using EquipmentRepairDocument.Core.DBContext;
using EquipmentRepairDocument.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRepairDocument.Core.Service.Users
{
    public class UserService(AppDbContext dbContext, IUserValidator userValidator, IPasswordHasher passwordHasher)
    {
        public async Task AddAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            BusinessLogicException.ThrowIfNull(username);
            BusinessLogicException.ThrowIfNull(password);

            if (!userValidator.ValidateUsername(username, out var messageValidUsername))
            {
                throw new BusinessLogicException(messageValidUsername);
            }

            if (!userValidator.ValidatePassword(password, out var messageValidPass))
            {
                throw new BusinessLogicException(messageValidPass);
            }

            if (dbContext.Users.Any(e => e.Username == username))
            {
                throw new BusinessLogicException($"This username {username} exists.");
            }

            var passwordHash = Hash(password);
            var user = new User(username, passwordHash);

            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public User? GetUser(string username, string passwordHash)
            => dbContext.Users.AsNoTracking().FirstOrDefault(e => e.Username == username && e.PasswordHash == passwordHash);

        public bool IsFreeUsername(string username)
            => dbContext.Users.AsNoTracking().FirstOrDefault(e => e.Username == username) == null;

        private string Hash(string password) => passwordHasher.Hash(password);
    }
}
