using System.Diagnostics.CodeAnalysis;

namespace EquipmentRepairDocument.Core.Exceptions
{
    public class BusinessLogicException : ApplicationException
    {
        public BusinessLogicException(string message)
            : base(message)
        {
        }

        public BusinessLogicException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public static void EnsureRange<T>(byte value, string propertyName, byte minLenght, byte maxLenght)
        {
            if (value <= maxLenght && value >= minLenght)
            {
                return;
            }

            OutOfRange<T>(propertyName, minLenght, maxLenght);
        }

        public static void EnsureLength<T>(string property, string propertyName, byte minLenght, byte maxLenght)
        {
            if (property.Length <= maxLenght && property.Length >= minLenght)
            {
                return;
            }

            StringLengthOutOfRange<T>(propertyName, minLenght, maxLenght);
        }

        public static void ThrowIfNull([NotNull] object? obj)
        {
            if (obj is null)
            {
                throw new BusinessLogicException("The passed object is empty.");
            }
        }

        public static void ThrowIfNullOrEmpty([NotNull] string? str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new BusinessLogicException("The passed string is empty.");
            }
        }

        public static void EnsureUniqueProperty<T>(byte property) => ThrowUniquePropertyError<T>(property);

        public static void EnsureUniqueProperty<T>(string property) => ThrowUniquePropertyError<T>(property);

        private static void ThrowUniquePropertyError<T>(object property) =>
            throw new BusinessLogicException($"{typeof(T).Name} \"{property}\" must be unique. A record with this value already exists.");

        private static BusinessLogicException OutOfRange<T>(string property, byte minLenght, byte maxLenght)
            => new BusinessLogicException($"The {property} {typeof(T).Name} is out of range [{minLenght}...{maxLenght}].");

        private static BusinessLogicException StringLengthOutOfRange<T>(string property, byte minLenght, byte maxLenght)
            => new BusinessLogicException($"The {property} {typeof(T).Name} the length of characters is not in the range [{minLenght}...{maxLenght}].");
    }
}
