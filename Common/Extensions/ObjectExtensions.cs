using System;

namespace Common.Extensions
{
    /// <summary>
    /// Adds extension methods for object.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Casts value to another type using the standard cast operator: (T)x
        /// </summary>
        /// <typeparam name="T">The type to cast value to.</typeparam>
        /// <param name="value">The object to cast.</param>
        /// <returns>Value casted to type T.</returns>
        /// <exception cref="InvalidCastException">If value cannot be cast to type T, this exception is thrown.</exception>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public static T CastTo<T>(this object value)
        {
            // Cannot use ChangeType if value is not IConvertible.
            // Simple (T)x cast does not work if the type has implicit/explicit casting but does not implement IConvertible.
            // In that case, use the explicit cast with dynamic 'boxing' which resolves the explicit casting at runtime.
            // http://stackoverflow.com/questions/11934134/generic-explicit-cast-failure-c-sharp
            // http://stackoverflow.com/questions/16114226/generics-explicit-conversion
            return (T)(value.ThrowIfCannotCast(typeof(T)).GetType().IsCastableTo(typeof(IConvertible)) ?
                Convert.ChangeType(value, typeof(T)) : (dynamic)value);
        }

        /// <summary>
        /// Casts value to another type unless the cast cannot be made.
        /// If it cannot be casted, it returns the default value of the type.
        /// For more info, see <see cref="TypeExtensions.GetDefault"/>.
        /// </summary>
        /// <typeparam name="T">The type to cast value to.</typeparam>
        /// <param name="value">The object to cast.</param>
        /// <returns>Value casted to type T. If the cast will not work, the default value of the type is returned.</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public static T CastToOrDefault<T>(this object value)
        {
            return value.ThrowIfNull("cast value").GetType().IsCastableTo(typeof(T)) ? value.CastTo<T>() : typeof(T).GetDefault<T>();
        }

        /// <summary>
        /// Checks to see if value is null.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is null. False otherwise.</returns>
        public static bool IsNull(this object value)
        {
            return (value == null);
        }
    }
}