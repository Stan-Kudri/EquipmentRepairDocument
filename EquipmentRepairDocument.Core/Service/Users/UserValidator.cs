namespace EquipmentRepairDocument.Core.Service.Users
{
    public class UserValidator : IUserValidator
    {
        public const int MinLengthUsername = 6;

        public const int MinLengthPass = 6;

        public bool ValidateUsername(string username, out string message)
        {
            message = string.Empty;

            if (username.Length < MinLengthUsername)
            {
                message = "Username is too short.";
                return false;
            }

            if (!username.All(e => char.IsLetterOrDigit(e)))
            {
                message = "Invalid characters in username.";
                return false;
            }

            return true;
        }

        public bool ValidatePassword(string password, out string message)
        {
            message = string.Empty;

            if (password == null)
            {
                message = "Password should not be empty.";
                return false;
            }

            if (password.Length < MinLengthPass)
            {
                message = "Password is too short.";
                return false;
            }

            if (!password.All(char.IsLetterOrDigit))
            {
                message = "Invalid characters in password.";
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                message = string.Format("{0}{1}{2}", "Invalid string format.", Environment.NewLine, "The password does not contain a number.");
                return false;
            }

            if (!password.Any(char.IsLetter))
            {
                message = string.Format("{0}{1}{2}", "Invalid string format.", Environment.NewLine, "The password does not contain a latter.");
                return false;
            }

            return true;
        }
    }
}
