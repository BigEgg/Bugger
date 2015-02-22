using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BigEgg.Framework.Utils
{
    /// <summary>
    /// Extends the <see cref="object"/> and the <see cref="XElement"/> with new serialize and deserialize methods.
    /// </summary>
    public static class XmlSerializeExtensions
    {
        /// <summary>
        /// Serialize the <see cref="object"/> to a <see cref="XElement"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        public static XElement ObjectToXElement<T>(this T obj) where T : class
        {
            XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, obj, xns);
                ms.Position = 0;

                using (XmlReader reader = XmlReader.Create(ms))
                {
                    return XElement.Load(reader);
                }
            }
        }

        /// <summary>
        /// Deserialize the <see cref="XElement"/> to a <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="element">The XElement to deserialize.</param>
        public static T XElementToObject<T>(this XElement element) where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(XElement));

                serializer.Serialize(ms, element);
                ms.Position = 0;

                serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(ms) as T;
            }
        }
    }
}
