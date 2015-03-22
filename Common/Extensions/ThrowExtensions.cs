using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    /// <summary>
    /// Provides extension methods for throwing exceptions based on certain conditions.
    /// </summary>
    public static class ThrowExtensions
    {
        /// <summary>
        /// Throws if the value is outside the specified range.
        /// </summary>
        /// <typeparam name="T">The type of value. Needs to be a struct and IComparable.</typeparam>
        /// <param name="value">The value for validation.</param>
        /// <param name="name">The name of the value if the validation fails.</param>
        /// <param name="lowerBound">The lower bound of the range. Can be set to null if no lower bound is defined.</param>
        /// <param name="upperBound">The upper bound of the range. Can be set to null if no upper bound is defined.</param>
        /// <returns>The value itself. If the value is not within the lower and/or upper bound,
        /// this method throws an ArgumentOutOfRangeException. If no bounds are defined (both null), then this method simply
        /// returns the value since no validation can be made.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value does not satisfy the provided bound(s).</exception>
        public static T ThrowIfOutOfRange<T>(this T value, string name, T? lowerBound, T? upperBound) where T : struct, IComparable
        {
            // If no bounds are present, the whole if statement is skipped.
            if (((lowerBound != null) && (value.CompareTo(lowerBound.Value) < 0)) ||
                ((upperBound != null) && (value.CompareTo(upperBound.Value) > 0)))
            {
                throw new ArgumentOutOfRangeException(String.Format("The {1} {0} is {2}.", value, name,
                    // {2} is being replaced by different formated strings depending on if the lower
                    // or the upper bound is present.
                    (((lowerBound != null) && (upperBound != null)) ?
                        String.Format("not between {0} and {1}", lowerBound.Value, upperBound.Value) :
                        ((lowerBound != null) ?
                        String.Format("less than {0}", lowerBound.Value) :
                        String.Format("greater than {0}", upperBound.Value)))));
            }

            return value;
        }

        /// <summary>
        /// Throws if the value is null.
        /// </summary>
        /// <typeparam name="T">The class-type of value.</typeparam>
        /// <param name="value">The value for validation.</param>
        /// <param name="name">The name of the value if the validation fails.</param>
        /// <returns>The value itself unless the value is null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
        public static T ThrowIfNull<T>(this T value, string name)
        {
            if (value.IsNull())
            {
                throw new ArgumentNullException("value", String.Format("The {0} is null.", name));
            }

            return value;
        }

        /// <summary>
        /// Throws if the value is null.
        /// </summary>
        /// <typeparam name="T">The class-type of value.</typeparam>
        /// <param name="value">The value for validation.</param>
        /// <returns>The value itself unless the value is null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the value is null.</exception>
        public static T ThrowIfNull<T>(this T value)
        {
            return value.ThrowIfNull(typeof(T).ToString());
        }

        /// <summary>
        /// Throws if the type T is not an enum type.
        /// </summary>
        /// <typeparam name="T">The type to check to see if it is an enum.</typeparam>
        /// <exception cref="ArgumentException">Thrown if the type T is not an enum type.</exception>
        public static void ThrowIfNotEnum<T>()
        {
            typeof(T).ThrowIfNotEnum();
        }

        /// <summary>
        /// Throws if the value is not an enum type.
        /// </summary>
        /// <typeparam name="T">The type to check to see if it is an enum.</typeparam>
        /// <param name="value">The actual value of the supposed enum.</param>
        /// <returns>The value itself unless the value type is not a enum.</returns>
        /// <exception cref="ArgumentException">Thrown if the value is not an enum type.</exception>
        public static T ThrowIfNotEnum<T>(this T value)
        {
            // This check is required when the type of value is, for example, an interface.
            // http://stackoverflow.com/a/9194376/294804
            return ThrowIfNotEnum(() => !(value is Enum), value, typeof(T));
        }

        /// <summary>
        /// Throws if the type is not an enum type.
        /// </summary>
        /// <param name="type">The type of the supposed enum.</param>
        /// <returns>Returns the type if it is an enum type.</returns>
        /// <exception cref="ArgumentException">Thrown if the type is not an enum type.</exception>
        public static Type ThrowIfNotEnum(this Type type)
        {
            return ThrowIfNotEnum(() => !type.IsEnum, type, type);
        }

        private static T ThrowIfNotEnum<T>(this Func<bool> condition, T value, Type type)
        {
            if (condition())
            {
                throw new ArgumentException(string.Format("T must be an enumerated type. T: {0}", type.FullName));
            }

            return value;
        }

        /// <summary>
        /// Throws if type T is not castable to type TCast.
        /// </summary>
        /// <typeparam name="T">The type to try to cast.</typeparam>
        /// <typeparam name="TCast">The type to be casted to.</typeparam>
        /// <returns>The type T if castable to type TCast.</returns>
        /// <exception cref="InvalidCastException">Thrown if type T cannot be cast to type TCast.</exception>
        public static void ThrowIfCannotCast<T, TCast>()
        {
            typeof(T).ThrowIfCannotCast(typeof(TCast));
        }

        /// <summary>
        /// Throws if value is not castable to type.
        /// </summary>
        /// <param name="value">The value to try to cast.</param>
        /// <param name="type">The type to be casted to.</param>
        /// <returns>The value if castable to type.</returns>
        /// <exception cref="InvalidCastException">Thrown if value cannot be cast to type.</exception>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        public static T ThrowIfCannotCast<T>(this T value, Type type)
        {
            // This call gets the boxed type of value, not the real type.
            //typeof(T).ThrowIfCannotCast(type);

            // This gets the real type of value.
            value.ThrowIfNull("cast value").GetType().ThrowIfCannotCast(type);
            return value;
        }

        /// <summary>
        /// Throws if typeFrom is not castable to typeTo.
        /// </summary>
        /// <param name="typeFrom">The type to try to cast.</param>
        /// <param name="typeTo">The type to be casted to.</param>
        /// <returns>The type of typeFrom if castable to typeTo.</returns>
        /// <exception cref="InvalidCastException">Thrown if typeFrom cannot be cast to typeTo.</exception>
        /// <exception cref="ArgumentNullException">Thrown if either of the types are null.</exception>
        public static Type ThrowIfCannotCast(this Type typeFrom, Type typeTo)
        {
            if (!typeFrom.ThrowIfNull("from-type").IsCastableTo(typeTo.ThrowIfNull("to-type")))
            {
                throw new InvalidCastException(String.Format("Cannot cast type {0} to type {1}.", typeFrom, typeTo));
            }

            return typeFrom;
        }
    }
}