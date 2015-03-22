using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Extensions
{
    /// <summary>
    /// Provides extension methods for Types.
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, IEnumerable<Type>> PrimitiveTypeTable = new Dictionary<Type, IEnumerable<Type>>
        {
            { typeof(decimal), new[] { typeof(long), typeof(ulong) } },
            { typeof(double), new[] { typeof(float) } },
            { typeof(float), new[] { typeof(long), typeof(ulong) } },
            { typeof(ulong), new[] { typeof(uint) } },
            { typeof(long), new[] { typeof(int), typeof(uint) } },
            { typeof(uint), new[] { typeof(byte), typeof(ushort) } },
            { typeof(int), new[] { typeof(sbyte), typeof(short), typeof(ushort) } },
            { typeof(ushort), new[] { typeof(byte), typeof(char) } },
            { typeof(short), new[] { typeof(byte) } }
        };

        private static bool IsPrimitiveCastableTo(this Type fromType, Type toType)
        {
            var keyTypes = new Queue<Type>(new[] { toType });
            while (keyTypes.Any())
            {
                var key = keyTypes.Dequeue();
                if (key == fromType) { return true; }
                if (PrimitiveTypeTable.ContainsKey(key)) { PrimitiveTypeTable[key].ToList().ForEach(keyTypes.Enqueue); }
            }
            return false;
        }

        /// <summary>
        /// Determines if this type is castable to the toType.
        /// This method does more than the is-operator and
        /// allows for primitives and implicit/explicit conversions to be compared properly.
        /// http://stackoverflow.com/a/18256885/294804
        /// </summary>
        /// <param name="fromType">The type to cast from.</param>
        /// <param name="toType">The type to be casted to.</param>
        /// <returns>True if fromType can be casted to toType. False otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if either type is null.</exception>
        public static bool IsCastableTo(this Type fromType, Type toType)
        {
            // http://stackoverflow.com/a/10416231/294804
            return toType.ThrowIfNull("to-type").IsAssignableFrom(fromType.ThrowIfNull("from-type")) ||
                fromType.IsPrimitiveCastableTo(toType) ||
                fromType.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(m =>
                    m.ReturnType == toType && m.Name == "op_Implicit" || m.Name == "op_Explicit");
        }

        /// <summary>
        /// Gets the default value for a given type.
        /// http://stackoverflow.com/a/353073/294804
        /// If the type is a numeric, the default constructed numeric is returned.
        /// If the type is a reference type, null is returned.
        /// </summary>
        /// <param name="type">The type to create a default value of.</param>
        /// <returns>A default value for the provided type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if type is null.</exception>
        public static object GetDefault(this Type type)
        {
            return type.ThrowIfNull().IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Gets the default value for a given type.
        /// http://stackoverflow.com/a/353073/294804
        /// If the type is a numeric, the default constructed numeric is returned.
        /// If the type is a reference type, null is returned.
        /// </summary>
        /// <param name="type">The type to create a default value of.</param>
        /// <returns>A default value for the provided type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if type is null.</exception>
        /// <exception cref="InvalidCastException">Thrown if type cannot be cast to T.</exception>
        public static T GetDefault<T>(this Type type)
        {
            var @default = type.GetDefault();
            // This check is required since CastTo throws if the value is null.
            return (@default == null) ? (T)(object)null : @default.CastTo<T>();
        }
    }
}