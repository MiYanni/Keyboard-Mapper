using System;
using System.Collections.Generic;

namespace Common.Extensions
{
    /// <summary>
    /// Methods to aid in handling numerics.
    /// </summary>
    public static class NumericExtensions
    {
        /// <summary>
        /// Counts the number of digits in a non-decimal number.
        /// If value is negative, the negative sign is not counted as a digit.
        /// </summary>
        /// <param name="value">The value to count the number of digits of.</param>
        /// <returns>The number of digits of value.</returns>
        public static int GetDigitCount(this long value)
        {
            if (value == 0) return 1;

            var count = 0;
            while (value != 0)
            {
                ++count;
                value /= 10;
            }
            return count;
        }

        /// <summary>
        /// Gets the digits of a non-decimal number as a list of bytes.
        /// </summary>
        /// <param name="value">The value to parse the digits out of.</param>
        /// <returns>A list of bytes where the least significant digit is the last byte of the list.</returns>
        public static IEnumerable<byte> GetDigits(this long value)
        {
            if (value == 0) return new List<byte> { 0 };

            var digits = new List<byte>();
            while (value != 0)
            {
                digits.Insert(0, (byte)(value % 10));
                value /= 10;
            }
            return digits;
        }

        /// <summary>
        /// Determines if the value is divisible evenly by divisor.
        /// </summary>
        /// <param name="value">The dividend to test.</param>
        /// <param name="divisor">The divisor to test.</param>
        /// <returns>True if value is evenly divisible by divisor. False otherwise.</returns>
        public static bool IsDivisibleBy(this int value, int divisor)
        {
            return (divisor != 0) && ((value % divisor) == 0);
        }
    }
}