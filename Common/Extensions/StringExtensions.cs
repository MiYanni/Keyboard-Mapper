using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Common.Extensions
{
    /// <summary>
    /// Provides extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Creates a string based on a list of elements.
        /// </summary>
        /// <param name="elements">The list of elements to convert to a string.</param>
        /// <param name="elementDelimiter">The delimiter between the elements.</param>
        /// <param name="showCount">Outputs the count of elements to the string.</param>
        /// <returns>The string representation of the list of elements.</returns>
        public static string ToElementString(this IEnumerable elements, string elementDelimiter = ",", bool showCount = false)
        {
            if (elements == null) return String.Empty;

            var elementStrings = new List<string>();
            var elementList = elements.Cast<object>().ToList();
            if (showCount) elementStrings.Add(String.Join(elementList.Count.ToString(CultureInfo.InvariantCulture), "{Count=", "}"));

            elementStrings.AddRange(elementList.Select(e =>
                (e == null) ? "null" :
                (e is byte) ? String.Format("0x{0:X2}", e) :
                e.ToString()
            ));

            return String.Join(elementDelimiter, elementStrings);
        }

        /// <summary>
        /// Creates a string based on an IEnumerable of string-object pairs.
        /// </summary>
        /// <param name="pairs">The IEnumerable of string-object pairs to be converted to a string.</param>
        /// <param name="pairDelimiter">The delimiter used between pairs.</param>
        /// <param name="valueDelimiter">The delimiter used between the name and value of the pair.</param>
        /// <returns>The string representation of the IEnumerable of string-object pairs.</returns>
        public static string ToNameValueString(this IEnumerable<KeyValuePair<string, object>> pairs, string pairDelimiter = ", ", string valueDelimiter = ":")
        {
            if (pairs == null) return String.Empty;

            return String.Join(
                String.Join(pairDelimiter, pairs.Select(dv =>
                    String.Join(valueDelimiter, dv.Key ?? String.Empty, dv.Value ?? "null"))),
                "[", "]");
        }

        /// <summary>
        /// Creates a string based on an IEnumerable of string-object pairs.
        /// </summary>
        /// <param name="pairs">The IEnumerable of string-object pairs to be converted to a string.</param>
        /// <param name="pairDelimiter">The delimiter used between pairs.</param>
        /// <param name="valueDelimiter">The delimiter used between the name and value of the pair.</param>
        /// <returns>The string representation of the IEnumerable of string-object pairs.</returns>
        public static string ToNameValueString(this IEnumerable<Tuple<string, object>> pairs, string pairDelimiter = ", ", string valueDelimiter = ":")
        {
            if (pairs == null) return String.Empty;

            return pairs.Where(nv => nv != null)
                .Select(nv => new KeyValuePair<string, object>(nv.Item1, nv.Item2)).ToNameValueString(pairDelimiter, valueDelimiter);
        }

        // http://stackoverflow.com/a/1650895/294804
        private static bool IsAnonymous(this Type type)
        {
            return type != null && type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Any() &&
                type.FullName != null && type.FullName.Contains("AnonymousType");
        }

        /// <summary>
        /// Convert an object to a string using reflection.
        /// Concept from here:
        /// http://chrisbenard.net/2009/07/23/using-reflection-to-dynamically-generate-tostring-output/
        /// http://stackoverflow.com/a/9299333/1824821
        /// </summary>
        /// <param name="this">The object to convert to a string.</param>
        /// <param name="propertyDelimiter">The delimiter used between properties in the string.</param>
        /// <param name="valueDelimiter">The delimiter used between the property name and value in the string.</param>
        /// <returns>A string representation of the object.</returns>
        public static string ToReflectedString(this object @this, string propertyDelimiter = ", ", string valueDelimiter = ":", bool hasClassName = true)
        {
            if (@this == null) return String.Empty;

            var thisType = @this.GetType();
            var typeName = thisType.IsAnonymous() ? "AnonymousType" : thisType.Name;
            var propertyStrings = hasClassName ? new List <string> { String.Join(valueDelimiter, "Class", typeName) } : new List<string>();
            var propertyArray = thisType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            propertyStrings.AddRange(propertyArray.Where(p => !p.GetIndexParameters().Any()).Select(p =>
            {
                var value = p.GetValue(@this, null);
                var valueAsString =
                    (value == null) ? "null" :
                    value.IsIEnumerableWithNoToString() ? (value as IEnumerable).ToElementString() :
                    p.PropertyType.IsEnum ? (value as IConvertible).ToEnumString() :
                    value.ToString();
                return String.Join(valueDelimiter, p.Name, valueAsString);
            }));

            if (@this.IsIEnumerableWithNoToString() && propertyArray.All(p => p.Name != "Items"))
            {
                propertyStrings.Add(String.Join(valueDelimiter, "Items", (@this as IEnumerable).ToElementString()));
            }

            return String.Join(propertyDelimiter, propertyStrings);
        }

        // Checks to see if value is castable to IEnumerable and if the property does not have an override for the ToString method.
        // http://stackoverflow.com/questions/7507609/is-there-a-way-to-tell-if-an-object-has-implemented-tostring-explicitly-in-c-sha
        // http://stackoverflow.com/questions/5005286/from-a-listobject-tostring-them-just-when-they-are-primitive-types-or-have
        private static bool IsIEnumerableWithNoToString(this object @this)
        {
            return !(@this is string) &&
                (@this is IEnumerable) &&
                (@this.GetType().GetMethod("ToString", BindingFlags.DeclaredOnly, null, Type.EmptyTypes, null) == null);
        }

        /// <summary>
        /// Returns the string with double quotes at the beginning and end of the string.
        /// </summary>
        /// <param name="text">The string to add quotes to.</param>
        /// <returns>The string with quotes around it.</returns>
        public static string ToQuotedString(this string text)
        {
            return String.Join(text, "\"", "\"");
        }

        /// <summary>
        /// Creates a string based on an enum value.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The value of the enum.</param>
        /// <param name="elementDelimiter">The delimiter between the enum as a name and the enum as a value.</param>
        /// <returns>A string that represents the enum value by name and by integer value.</returns>
        public static string ToEnumString<T>(this T value, string elementDelimiter = "=") where T : IConvertible
        {
            value.ThrowIfNotEnum();
            return String.Join(elementDelimiter, value, Convert.ToInt32(value));
        }

        /// <summary>
        /// Takes a camel-cased string (ExampleString), and creates a string delimited by spaces (Example String).
        /// This Regex puts spaces between the camel casing.
        /// http://stackoverflow.com/a/3622700/1824821
        /// </summary>
        /// <param name="camelCaseString">The camel-cased string to space out.</param>
        /// <returns>A string that has spaces between the casing in the passed-in string.</returns>
        public static string ToSpacedStringFromCamelCase(this string camelCaseString)
        {
            return Regex.Replace(camelCaseString,
                @"(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))",
                @" ${a}");
        }

        /// <summary>
        /// Check to see if value is contained within source. Allows for a StringComparison to be used.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="value">The value to check to see if it is contained within the source.</param>
        /// <param name="comparer">The string comparer. To ignore case, pass in StringComparison.OrdinalIgnoreCase.</param>
        /// <returns>True if value is contained within source. False otherwise.</returns>
        public static bool Contains(this string source, string value, StringComparison comparer)
        {
            return source.IndexOf(value, comparer) >= 0;
        }
    }
}