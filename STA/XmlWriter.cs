using System.IO;
using System.Xml.Serialization;

namespace STA
{
    /// <summary>
    /// Writes mine topology data to the file.
    /// </summary>
    internal class XmlWriter
    {
        /// <summary>
        /// Writes the specified mine topology to the file.
        /// </summary>
        /// <typeparam name="T">The type of the serializing object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="fileName">Name of the output file.</param>
        public static void Write<T>(T obj, string fileName)
        {
            // Serialize mine to XML.
            var serializer = new XmlSerializer(typeof(T));

            // Omitting namespaces.
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            // Serialization.
            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, obj, ns);
            }
        }
    }
}
