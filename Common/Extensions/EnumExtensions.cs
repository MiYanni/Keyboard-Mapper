using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Extensions
{
    /// <summary>
    /// Provides extension methods for Enums.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns all the values of an enum type T.
        /// </summary>
        /// <typeparam name="T">An enum type.</typeparam>
        /// <returns>An IEnumerable of the values of the enum type.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static IEnumerable<T> AllValues<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T).ThrowIfNotEnum()).Cast<T>();
        }

        /// <summary>
        /// Gets the name of the enum value.
        /// </summary>
        /// <typeparam name="T">An enum type.</typeparam>
        /// <param name="value">The enum value to get the name of.</param>
        /// <returns>A string of the name of the enum value.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static string GetName<T>(this T value) where T : struct, IConvertible
        {
            return Enum.GetName(typeof(T).ThrowIfNotEnum(), value);
        }

        /// <summary>
        /// Determines if otherValue exists in value for a Flag enum.
        /// Assumes the max value of the enum is a 32-bit integer.
        /// </summary>
        /// <param name="value">The value to check against.</param>
        /// <param name="otherValue">The other value to see if it exists in value.</param>
        /// <typeparam name="T">A flag enum type.</typeparam>
        /// <returns>Returns true if other exists within value.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static bool HasFlag<T>(this T value, T otherValue) where T : struct, IConvertible
        {
            typeof(T).ThrowIfNotEnum();

            // Assuming a 32-bit integer.
            var valueInt = Convert.ToInt32(value);
            var otherValueInt = Convert.ToInt32(otherValue);

            return (valueInt & otherValueInt) == otherValueInt;
        }

        /// <summary>
        /// Determines if all otherValues exists in value.
        /// Assumes the max value of the enum is a 32-bit integer.
        /// </summary>
        /// <typeparam name="T">A flag enum type.</typeparam>
        /// <param name="value">The value to check against.</param>
        /// <param name="otherValues">The other values to see if they exist in value.</param>
        /// <returns>Returns true if all other values exist within value.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static bool HasFlags<T>(this T value, params T[] otherValues) where T : struct, IConvertible
        {
            typeof(T).ThrowIfNotEnum();

            return otherValues.All(other => HasFlag(value, other));
        }

        /// <summary>
        /// Returns all the flags as a single value.
        /// Assumes the max value of the enum is a 32-bit integer.
        /// </summary>
        /// <typeparam name="T">A flag enum type.</typeparam>
        /// <returns>The value that represents every value of the flag enum.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static T AllFlags<T>() where T : struct, IConvertible
        {
            return AllFlagsExcept<T>();
        }

        /// <summary>
        /// Returns all the flags as a single value excluding the exceptions provided.
        /// Assumes the max value of the enum is a 32-bit integer.
        /// </summary>
        /// <param name="exceptions">The enum values to not include in the value.</param>
        /// <typeparam name="T">A flag enum type.</typeparam>
        /// <returns>The value that represents every value of the flag enum excluding exceptions.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static T AllFlagsExcept<T>(params T[] exceptions) where T : struct, IConvertible
        {
            return CombineFlags(AllValues<T>().Except(exceptions));
        }

        /// <summary>
        /// Splits an enum that contains multiple flag values into an enumerable of individual flag values.
        /// Assumes the max value of the enum is a 32-bit integer.
        /// </summary>
        /// <param name="flags">The flags to split into individual flags.</param>
        /// <typeparam name="T">A flag enum type.</typeparam>
        /// <returns>An enumerable of the individual flag values from flags.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static IEnumerable<T> SplitFlags<T>(this T flags) where T : struct, IConvertible
        {
            return AllValues<T>().Where(flag => HasFlag(flags, flag));
        }

        /// <summary>
        /// Combines multiple flag values into an single enum that contains multiple flag values excluding exceptional values (optional).
        /// Assumes the max value of the enum is a 32-bit integer.
        /// </summary>
        /// <param name="flags">The flags to split into individual flags.</param>
        /// <typeparam name="T">A flag enum type.</typeparam>
        /// <returns>A single enum value that contains the values of flags.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        /// <exception cref="ArgumentNullException">Thrown if flags is null.</exception>
        public static T CombineFlags<T>(this IEnumerable<T> flags) where T : struct, IConvertible
        {
            typeof(T).ThrowIfNotEnum();

            // Assuming a 32-bit integer.
            var flagsAsInt = flags.ThrowIfNull().Select(v => Convert.ToInt32(v)).ToList();
            return flagsAsInt.Any() ? (T)Enum.ToObject(typeof(T), flagsAsInt.Aggregate((all, next) => all | next)) : default(T);
        }

        /// <summary>
        /// Gets the 0-based index of the set bit starting from the LSB for the flag.
        /// If -1 is returned, the flag provided was 0.
        /// Assumes the max value of the enum is a 32-bit integer.
        /// This method defaults to finding the lowest set bit.
        /// </summary>
        /// <typeparam name="T">A flag enum type.</typeparam>
        /// <param name="flag">The flag to detect the set bit index.</param>
        /// <returns>The index of the set bit.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static int GetFlagBitIndex<T>(this T flag) where T : struct, IConvertible
        {
            return GetFlagBitIndexLowest(flag);
        }

        /// <summary>
        /// Gets the 0-based index of the set bit (lowest) starting from the LSB for the flag.
        /// If -1 is returned, the flag provided was 0.
        /// Assumes the max value of the enum is a 32-bit integer.
        /// </summary>
        /// <typeparam name="T">A flag enum type.</typeparam>
        /// <param name="flag">The flag to detect the set bit index.</param>
        /// <returns>The (lowest) index of the set bit.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static int GetFlagBitIndexLowest<T>(this T flag) where T : struct, IConvertible
        {
            typeof(T).ThrowIfNotEnum();

            // Assuming a 32-bit integer.
            var flagInt = Convert.ToInt32(flag);
            for (var index = 0; index < 32; ++index)
            {
                if ((flagInt & (1 << index)) != 0)
                {
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the 0-based index of the set bit (highest) starting from the LSB for the flag.
        /// If -1 is returned, the flag provided was 0.
        /// Assumes the max value of the enum is a 32-bit integer.
        /// </summary>
        /// <typeparam name="T">A flag enum type.</typeparam>
        /// <param name="flag">The flag to detect the set bit index.</param>
        /// <returns>The (highest) index of the set bit.</returns>
        /// <exception cref="ArgumentException">Thrown if T is not an Enum type.</exception>
        public static int GetFlagBitIndexHighest<T>(this T flag) where T : struct, IConvertible
        {
            typeof(T).ThrowIfNotEnum();

            // Assuming a 32-bit integer.
            var flagInt = Convert.ToInt32(flag);
            var index = -1;
            while (flagInt != 0)
            {
                flagInt >>= 1;
                ++index;
            }
            return index;
        }
    }
}