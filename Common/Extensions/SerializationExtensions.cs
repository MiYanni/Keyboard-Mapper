using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;

namespace Common.Extensions
{
    /// <summary>
    /// Provides extension methods for serializing and deserializing objects.
    /// </summary>
    public static class SerializationExtensions
    {
        /// <summary>
        /// Serializes an object to a binary representation.
        /// Based on Nito.Async's implementation.
        /// </summary>
        /// <param name="item">The object to serialize. Must implement ISerializable.</param>
        public static byte[] Serialize(this object item)
        {
            var type = item.ThrowIfNull("item to serialize").GetType();
            if (!type.IsSerializable)
            {
                throw new ArgumentException(String.Format("The type {0} does not implement ISerializable.", type));
            }

            using (var stream = new MemoryStream())
            {
                //new BinaryFormatter().Serialize(stream, new JavaScriptSerializer().Serialize(item));
                new BinaryFormatter().Serialize(stream, item);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deserializes an object from a binary representation to an object.
        /// Based on Nito.Async's implementation.
        /// </summary>
        /// <param name="serializedItem">The byte array to deserialize.</param>
        /// <returns>The item deserialized to an object.</returns>
        public static object Deserialize(this byte[] serializedItem)
        {
            if (!serializedItem.ThrowIfNull("serialized item").Any())
            {
                throw new ArgumentException("The serialized item does not contain any bytes to deserialize.");
            }

            using (var stream = new MemoryStream(serializedItem))
            {
                //return new JavaScriptSerializer().DeserializeObject((string)new BinaryFormatter().Deserialize(stream));
                return new BinaryFormatter().Deserialize(stream);
            }
        }

        /// <summary>
        /// Deserializes an object from a binary representation to type T.
        /// Based on: http://stackoverflow.com/questions/2619831/transmitting-complex-objects-using-tcp/2620355#2620355
        /// </summary>
        /// <typeparam name="T">The type of the serialized item.</typeparam>
        /// <param name="serializedItem">The byte array to deserialize.</param>
        /// <returns>The item deserialized to type T.</returns>
        public static T Deserialize<T>(this byte[] serializedItem)
        {
            return (T)Deserialize(serializedItem);
        }
    }
}